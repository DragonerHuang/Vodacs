using AutoMapper;
using Dm.util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Services;
using Vodace.Core.UserManager;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Sys_RoleService
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISys_RoleRepository _repository;
        private readonly ISys_User_NewRepository _User_NewRepository;
        private readonly IMapper _mapper;

        private WebResponseContent _responseContent = new WebResponseContent();
        public override PageGridData<Sys_Role> GetPageData(PageDataOptions pageData)
        {
            //角色Id=1默认为超级管理员角色，界面上不显示此角色
            QueryRelativeExpression = (IQueryable<Sys_Role> queryable) =>
            {
                if (UserContext.Current.IsSuperAdmin)
                {
                    return queryable;
                }
                List<int> roleIds = GetAllChildrenRoleIdAndSelf();
                return queryable.Where(x => roleIds.Contains(x.role_id));
            };
            return base.GetPageData(pageData);
        }
        /// <summary>
        /// 编辑权限时，获取当前用户的所有菜单权限
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentUserTreePermission()
        {
            return await GetUserTreePermission(UserContext.Current.RoleId);
        }

        /// <summary>
        /// 编辑权限时，获取指定角色的所有菜单权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetUserTreePermission(int roleId)
        {
            //if (!UserContext.IsRoleIdSuperAdmin(roleId) && UserContext.Current.RoleId != roleId)
            //{
            //    //if (!(await GetAllChildrenAsync(UserContext.Current.RoleId)).Exists(x => x.Id == roleId))
            //    //{
            //    //    return _responseContent.Error("没有权限获取此角色的权限信息");
            //    //}
            //    return _responseContent.Error("没有权限获取此角色的权限信息");
            //}
            //获取用户权限
            List<Permissions> permissions = UserContext.Current.GetPermissions(roleId);
            //权限用户权限查询所有的菜单信息
            List<Sys_Menu> menus = await Task.Run(() => Sys_MenuService.Instance.GetUserMenuList(roleId).Where(d=>d.menu_type ==3).ToList());
            //获取当前用户权限如:(Add,Search)对应的显示文本信息如:Add：添加，Search:查询
            var data = menus.Select(x => new
            {
                Id = x.menu_id,
                Pid = x.parent_id,
                Text = x.menu_name,
                Text_eng = x.menu_name_us,
                Text_cht=x.menu_name_tw,
                //IsApp = x.menu_type == 1,
                Actions = GetActions(x.menu_id, x.Actions, permissions, roleId)
            });
            return _responseContent.OK(null, data);
        }

        private List<Sys_Actions> GetActions(int menuId, List<Sys_Actions> menuActions, List<Permissions> permissions, int roleId)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return menuActions;
            }

            return menuActions.Where(p => permissions
                 .Exists(w => menuId == w.Menu_Id
                 && w.UserAuthArr.Contains(p.Value)))
                  .ToList();
        }

        private List<RoleNodes> rolesChildren = new List<RoleNodes>();

        /// <summary>
        /// 编辑权限时获取当前用户下的所有角色与当前用户的菜单权限
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentTreePermission()
        {
            _responseContent = await GetCurrentUserTreePermission();
            int roleId = UserContext.Current.RoleId;
            return _responseContent.OK(null, new
            {
                tree = _responseContent.data,
                roles = await GetAllChildrenAsync(roleId)
            });
        }

        public async Task<WebResponseContent> GetALLTreePermission() 
        {
            _responseContent = await GetCurrentUserTreePermission();
            return _responseContent;
        }

        public async Task<WebResponseContent> GetCurrentTreePermission(int roleId) 
        {
            _responseContent = await GetUserTreePermission(roleId);
            //return _responseContent.OK(null, new
            //{
            //    tree = _responseContent.data,
            //    roles = await GetAllChildrenAsync(roleId)
            //});
            return _responseContent.OK("Ok", _responseContent.data);
        }

        private List<RoleNodes> roles = null;

        public Sys_RoleService(ILocalizationService localizationService, ISys_RoleRepository repository, IMapper mapper, ISys_User_NewRepository user_NewRepository)
        {
            _localizationService = localizationService;
            _repository = repository;
            _mapper = mapper;
            _User_NewRepository = user_NewRepository;
        }

        /// <summary>
        /// 获取当前角色下的所有角色包括自己的角色Id
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllChildrenRoleIdAndSelf()
        {
            int roleId = UserContext.Current.RoleId;
            List<int> roleIds = GetAllChildren(roleId).Select(x => x.Id).ToList();
            roleIds.Add(roleId);
            return roleIds;
        }


        /// <summary>
        /// 获取当前角色下的所有角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<RoleNodes> GetAllChildren(int roleId)
        {
            roles = GetAllRoleQueryable(roleId).ToList();
            return GetAllChildrenNodes(roleId);
        }

        public async Task<List<RoleNodes>> GetAllChildrenAsync(int roleId)
        {
            roles = await GetAllRoleQueryable(roleId).ToListAsync();
            return GetAllChildrenNodes(roleId);
        }
        private IQueryable<RoleNodes> GetAllRoleQueryable(int roleId)
        {
            return _repository
                   .FindAsIQueryable(
                   x => x.enable == 1 && x.role_id > 1)
                   .Select(
                   s => new RoleNodes()
                   {
                       Id = s.role_id,
                       RoleName = s.role_name
                   });
        }

        public async Task<List<int>> GetAllChildrenRoleIdAsync(int roleId)
        {
            return (await GetAllChildrenAsync(roleId)).Select(x => x.Id).ToList();
        }


        public List<int> GetAllChildrenRoleId(int roleId)
        {
            return GetAllChildren(roleId).Select(x => x.Id).ToList();
        }

        private List<RoleNodes> GetAllChildrenNodes(int roleId)
        {
            return RoleContext.GetAllChildren(roleId);
        }
        /// <summary>
        /// 递归获取所有子节点权限
        /// </summary>
        /// <param name="roleId"></param>


        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="userPermissions"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SavePermission(List<UserPermissions> userPermissions, int roleId)
        {

            string message = "";
            try
            {
                UserInfo user = UserContext.Current.UserInfo;
                //if (!(await GetAllChildrenAsync(user.Role_Id)).Exists(x => x.Id == roleId))
                //    return _responseContent.Error("没有权限修改此角色的权限信息");
                //当前用户的权限
                List<Permissions> permissions = UserContext.Current.Permissions;

                List<int> originalMeunIds = new List<int>();
                //被分配角色的权限
                List<Sys_Role_Auth> roleAuths = await _repository.FindAsync<Sys_Role_Auth>(x => x.role_id == roleId);
                List<Sys_Role_Auth> updateAuths = new List<Sys_Role_Auth>();
                foreach (UserPermissions x in userPermissions)
                {
                    Permissions per = permissions.Where(p => p.Menu_Id == x.Id).FirstOrDefault();
                    //不能分配超过当前用户的权限
                    if (per == null) continue;
                    //per.UserAuthArr.Contains(a.Value)校验权限范围
                    string[] arr = x.Actions == null || x.Actions.Count == 0
                      ? new string[0]
                      : x.Actions.Where(a => per.UserAuthArr.Contains(a.Value))
                      .Select(s => s.Value).ToArray();

                    //如果当前权限没有分配过，设置Auth_Id默认为0，表示新增的权限
                    var auth = roleAuths.Where(r => r.menu_id == x.Id).Select(s => new { s.auth_id, s.auth_value, s.menu_id }).FirstOrDefault();
                    string newAuthValue = string.Join(",", arr);
                    //权限没有发生变化则不处理
                    if (auth == null || auth.auth_value != newAuthValue)
                    {
                        updateAuths.Add(new Sys_Role_Auth()
                        {
                            role_id = roleId,
                            menu_id = x.Id,
                            auth_value = string.Join(",", arr),
                            auth_id = auth == null ? 0 : auth.auth_id,
                            modify_date = DateTime.Now,
                            modify_name = user.UserTrueName,
                            create_date = DateTime.Now,
                            Create_name = user.UserTrueName
                        });
                    }
                    else
                    {
                        originalMeunIds.Add(auth.menu_id);
                    }

                }
                //更新权限
                _repository.UpdateRange(updateAuths.Where(x => x.auth_id > 0), x => new
                {
                    x.menu_id,
                    x.auth_value,
                    x.modify_date,
                    x.modify_name
                });
                //新增的权限
                _repository.AddRange(updateAuths.Where(x => x.auth_id <= 0));

                //获取权限取消的权限
                int[] authIds = roleAuths.Where(x => userPermissions.Select(u => u.Id)
                 .ToList().Contains(x.menu_id) || originalMeunIds.Contains(x.menu_id))
                .Select(s => s.auth_id)
                .ToArray();
                List<Sys_Role_Auth> delAuths = roleAuths.Where(x => x.auth_value != "" && !authIds.Contains(x.auth_id)).ToList();
                delAuths.ForEach(x =>
                {
                    x.auth_value = "";
                });
                //将取消的权限设置为""
                _repository.UpdateRange(delAuths, x => new
                {
                    x.menu_id,
                    x.auth_value,
                    x.modify_date,
                    x.modify_name
                });

                int addCount = updateAuths.Where(x => x.auth_id <= 0).Count();
                int updateCount = updateAuths.Where(x => x.auth_id > 0).Count();
                await _repository.SaveChangesAsync();

                string _version = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                //标识缓存已更新
                base.CacheContext.Add(roleId.GetRoleIdKey(), _version);

                _responseContent.OK($"保存成功：新增加配菜单权限{addCount}条,更新菜单{updateCount}条,删除权限{delAuths.Count()}条");
            }
            catch (Exception ex)
            {
                message = "异常信息：" + ex.Message + ex.StackTrace + ex.InnerException + ",";
                return _responseContent.Error(message);
            }
            finally
            {
                Logger.Info($"权限分配置:{message}{_responseContent.message}");
            }

            return _responseContent;
        }


        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            AddOnExecuting = (Sys_Role role, object obj) =>
            {
                return ValidateRoleName(role, x => x.role_name == role.role_name);
            };
            return RemoveCache(base.Add(saveDataModel));
        }

        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            if (!UserContext.Current.IsSuperAdmin)
            {
                var roleIds = RoleContext.GetAllChildrenIds(UserContext.Current.RoleId);
                var _keys = keys.Select(s => s.GetInt());
                if (_keys.Any(x => !roleIds.Contains(x)))
                {
                    return _responseContent.Error("没有权限删除此角色");
                }
            }

            return RemoveCache(base.Del(keys, delList));
        }

        private WebResponseContent ValidateRoleName(Sys_Role role, Expression<Func<Sys_Role, bool>> predicate)
        {
            if (_repository.Exists(predicate))
            {
                return _responseContent.Error($"角色名【{role.role_name}】已存在,请设置其他角色名");
            }
            return _responseContent.OK();
        }

        public override WebResponseContent Update(SaveModel saveModel)
        {
            UpdateOnExecuting = (Sys_Role role, object obj1, object obj2, List<object> obj3) =>
            {
                if (role.role_id == UserContext.Current.RoleId)
                {
                    return _responseContent.Error($"不能修改自己的角色");
                }
                return ValidateRoleName(role, x => x.role_name == role.role_name && x.role_id != role.role_id);
            };
            return RemoveCache(base.Update(saveModel));
        }
        private WebResponseContent RemoveCache(WebResponseContent webResponse)
        {
            if (webResponse.status)
            {
                RoleContext.Refresh();
            }
            return webResponse;
        }

        #region MyRegion
        public async Task<WebResponseContent> GetListByPage(PageInput<RoleQuery> query)
        {
            PageGridData<MenuListDto> pageGridData = new PageGridData<MenuListDto>();
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 && d.role_id !=1 &&
            (queryPam.enable ==null? true : d.enable == queryPam.enable)
            && (string.IsNullOrEmpty(queryPam.role_name) ? true : d.role_name.Contains(queryPam.role_name))).Select(d => new RoleListDto
            {
                role_id = d.role_id,
                role_name = d.role_name,
                enable = d.enable,
                create_date = d.create_date,
                create_name = d.create_name,
                modify_date = d.modify_date,
                modify_name = d.modify_name,
                remark = d.remark,
            });
            var result =await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }
        public async Task<WebResponseContent> GetRoleById(int id)
        {
            var data = await _repository.FindFirstAsync(d => d.delete_status == 0 && d.role_id == id);
            if (data == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
            else return WebResponseContent.Instance.OK(_localizationService.GetString("success"), data);
        }
        public WebResponseContent AddData(RoleDto role)
        {
            try
            {
                if (role == null) return WebResponseContent.Instance.Error("content_connot_be_empty");

                if (string.IsNullOrEmpty(role.role_name))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("role_name_null"));

                var model = _repository.FindAsIQueryable(x => x.delete_status == 0 && x.role_name == role.role_name).FirstOrDefault();
                if (model != null)
                {
                    //已存在
                    return WebResponseContent.Instance.Error(model.role_name + _localizationService.GetString("existent"));
                }
                else
                {
                    //未存在
                    Sys_Role sys_Role = _mapper.Map<Sys_Role>(role);
                    sys_Role.enable = role.enable;
                    sys_Role.delete_status = (int)SystemDataStatus.Valid;
                    sys_Role.create_date = DateTime.Now;
                    sys_Role.create_name = UserContext.Current.UserName;
                    _repository.Add(sys_Role, true);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), sys_Role);
                }

                //return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"{this.GetType().Name}_AddData异常：{ex.Message}");
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_AddData异常：{ex.Message}", ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent DelData(int role_id)
        {
            try
            {
                var model = _repository.FindAsIQueryable(x => x.role_id == role_id).FirstOrDefault();
                if (UserContext.Current.IsSuperAdmin)
                {
                    var checkIsUsing = _User_NewRepository.Exists(x => x.role_id == role_id && x.delete_status == (int)SystemDataStatus.Valid);
                    if(checkIsUsing) return WebResponseContent.Instance.Error(_localizationService.GetString("role_using"));

                    model.role_id = role_id;
                    model.delete_status =(int)SystemDataStatus.Invalid;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;
                    var res = _repository.Update(model, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                else
                {
                    //权限不足
                    return WebResponseContent.Instance.Error(model.role_name + _localizationService.GetString("insufficient_permissions"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"{this.GetType().Name}_DelData异常：{ex.Message}");
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_AddData异常：{ex.Message}", ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent EditData(EditRoleDto role)
        {
            try
            {
                var model = _repository.FindAsIQueryable(x => x.delete_status == 0 && x.role_name == role.role_name && role.role_id != role.role_id).FirstOrDefault();
                if (model != null)
                {
                    //已存在
                    return WebResponseContent.Instance.Error(model.role_name + _localizationService.GetString("existent"));
                }
                //未存在
                Sys_Role sys_Role = _repository.Find(d => d.role_id == role.role_id).FirstOrDefault(); //_mapper.Map<Sys_Role>(role);
                if (sys_Role != null) 
                {
                    sys_Role.role_name = role.role_name;
                    sys_Role.enable = role.enable;
                    sys_Role.remark = role.remark;
                    sys_Role.modify_date = DateTime.Now;
                    sys_Role.delete_status = (int)SystemDataStatus.Valid;
                    sys_Role.modify_name = UserContext.Current.UserName;
                    var res = _repository.Update(sys_Role, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                else return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"{this.GetType().Name}_EditData异常：{ex.Message}");
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_EditData异常：{ex.Message}", ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent SwitchEnable(int id, int enable)
        {
            try
            {
                if (id == 0) return WebResponseContent.Instance.Error("id_null");
                var entData = _repository.Find(d => d.role_id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.enable = (byte)enable;
                    entData.modify_date = DateTime.Now;
                    entData.modify_name = UserContext.Current.UserName;

                    var res = _repository.Update(entData, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed")); ;
            }
        }

        #endregion
    }


    public class UserPermissions
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Text { get; set; }
        //public bool IsApp { get; set; }
        public List<Sys_Actions> Actions { get; set; }
    }
}

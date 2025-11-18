using AutoMapper;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Services;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Entity.DomainModels.Common;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Sys_MenuService
    {
        private readonly ILocalizationService _localizationService;
        private readonly ISys_MenuRepository _repository;
        private readonly IMapper _mapper;
        /// <summary>
        /// 菜单静态化处理，每次获取菜单时先比较菜单是否发生变化，如果发生变化从数据库重新获取，否则直接获取_menus菜单
        /// </summary>
        private static List<Sys_Menu> _menus { get; set; }

        /// <summary>
        /// 从数据库获取菜单时锁定的对象
        /// </summary>
        private static object _menuObj = new object();

        /// <summary>
        /// 当前服务器的菜单版本
        /// </summary>
        private static string _menuVersionn = "";

        private const string _menuCacheKey = "inernalMenu";

        public Sys_MenuService(ILocalizationService localizationService, IMapper mapper, ISys_MenuRepository sys_MenuRepository)
        {
            _localizationService = localizationService;
            _mapper = mapper;
            _repository = sys_MenuRepository;
        }

        /// <summary>
        /// 编辑修改菜单时,获取所有菜单
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetMenu()
        {
            //  DBServerProvider.SqlDapper.q
            return (await _repository.FindAsync(x => 1 == 1, a =>
             new
             {
                 id = a.menu_id,
                 parentId = a.parent_id,
                 name = a.menu_name,
                 a.menu_type,
                 a.orderNo
             })).OrderByDescending(a => a.orderNo)
                .ThenByDescending(q => q.parentId).ToList();

        }

        private List<Sys_Menu> GetAllMenu()
        {
            //每次比较缓存是否更新过，如果更新则重新获取数据
            //string _cacheVersion = CacheContext.Get(_menuCacheKey);
            //if (_menuVersionn != "" && _menuVersionn == _cacheVersion)
            //{
            //    return _menus ?? new List<Sys_Menu>();
            //}
            //lock (_menuObj)
            //{
            //    if (_menuVersionn != "" && _menus != null && _menuVersionn == _cacheVersion) return _menus;
            //    //2020.12.27增加菜单界面上不显示，但可以分配权限
                _menus = _repository.FindAsIQueryable(x => (x.enable == 1 || x.enable == 2) && x.delete_status ==0)
                    .OrderBy(a => a.orderNo)
                    .ThenByDescending(q => q.parent_id).ToList();

                _menus.ForEach(x =>
                {
                    // 2022.03.26增移动端加菜单类型
                    x.menu_type ??= 0;
                    if (!string.IsNullOrEmpty(x.auth) && x.auth.Length > 10)
                    {
                        try
                        {
                            x.Actions = x.auth.DeserializeObject<List<Sys_Actions>>();
                        }
                        catch { }
                    }
                    if (x.Actions == null) x.Actions = new List<Sys_Actions>();
                });

            //    string cacheVersion = CacheContext.Get(_menuCacheKey);
            //    if (string.IsNullOrEmpty(cacheVersion))
            //    {
            //        cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
            //        CacheContext.Add(_menuCacheKey, cacheVersion);
            //    }
            //    else
            //    {
            //        _menuVersionn = cacheVersion;
            //    }
            //}
            return _menus;
        }

        /// <summary>
        /// 获取当前用户有权限查看的菜单
        /// </summary>
        /// <returns></returns>
        public List<Sys_Menu> GetCurrentMenuList()
        {
            int roleId = UserContext.Current.RoleId;
            return GetUserMenuList(roleId);
        }


        public List<Sys_Menu> GetUserMenuList(int roleId)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return GetAllMenu();
            }
            List<int> menuIds = UserContext.Current.GetPermissions(roleId).Select(x => x.Menu_Id).ToList();
            return GetAllMenu().Where(x => menuIds.Contains(x.menu_id)).ToList();
        }

        /// <summary>
        /// 获取当前用户所有菜单与权限
        /// </summary>
        /// <returns></returns>
        public object GetCurrentMenuActionList()
        {
            return GetMenuActionList(UserContext.Current.RoleId);
        }

        public object GetCurrentMenuActionListNew()
        {
            return GetMenuActionListNew(UserContext.Current.RoleId);
        }

        public object GetCurrentMenuActionLisAll()
        {
            return GetMenuActionListRecursive(UserContext.Current.RoleId);
        }

        /// <summary>
        /// 根据角色ID获取菜单与权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public object GetMenuActionListNew(int roleId)
        {
            var menu = new List<MenuTree>();
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                menu = GetAllMenu()
                .Where(c => c.menu_type == 3 && c.delete_status == 0)
                .Select(x =>
                new MenuTree
                {
                    id = x.menu_id,
                    name = x.menu_name,
                    path = x.url,
                    parentId = x.parent_id,
                    component = x.component,
                    menuNameTW = x.menu_name_tw,
                    menuNameUS = x.menu_name_us,
                    icon = x.icon,
                    description= x.description,
                    orderNo = x.orderNo,
                    hidden = x.hidden == 1 ? true : false,
                    parent_title = x.parent_title,
                    permission = x.Actions.Select(s => s.Value).ToArray()
                }).OrderBy(d => d.orderNo).ToList();
            }
            else
            {
                menu = (from a in UserContext.Current.Permissions
                        join b in GetAllMenu().Where(c => c.menu_type == 3 && c.delete_status == 0)
                        on a.Menu_Id equals b.menu_id
                        orderby b.orderNo descending
                        select new MenuTree
                        {
                            id = a.Menu_Id,
                            name = b.menu_name,
                            path = b.url,
                            parentId = b.parent_id,
                            component = b.component,
                            menuNameTW = b.menu_name_tw,
                            menuNameUS = b.menu_name_us,
                            description = b.description,
                            icon = b.icon,
                            orderNo = b.orderNo,
                            hidden = b.hidden == 1 ? true : false,
                            parent_title = b.parent_title,
                            permission = a.UserAuthArr
                        }).OrderBy(d => d.orderNo).ToList();
            }

            List<MenuTree> list = new List<MenuTree>();
            var parent_menu = menu.Where(d => d.parentId == 0);
            var children_menu = menu.Where(d => d.parentId > 0);
            foreach (var item in parent_menu) 
            {
                MenuTree menuTree = new MenuTree();
                menuTree.id = item.id;
                menuTree.name = item.name;
                menuTree.path = item.path;
                menuTree.component = item.component;
                menuTree.menuNameUS = item.menuNameUS;
                menuTree.menuNameTW = item.menuNameTW;
                menuTree.icon = item.icon;
                menuTree.permission = item.permission;
                menuTree.description = item.description;
                menuTree.hidden = item.hidden;
                menuTree.parent_title = item.parent_title;
                foreach (var citem in children_menu.Where(d=>d.parentId == item.id))
                {
                    MenuTree child = new MenuTree();
                    child.id = citem.id;
                    child.name = citem.name;
                    child.path = citem.path;
                    child.component = citem.component;
                    child.menuNameUS = citem.menuNameUS;
                    child.menuNameTW = citem.menuNameTW;
                    child.description = citem.description;
                    child.icon = citem.icon;
                    child.permission = citem.permission;
                    child.hidden = citem.hidden;
                    child.parent_title = citem.parent_title;
                    menuTree.children.Add(child);
                }
                list.Add(menuTree);
            }
            return WebResponseContent.Instance.OK("Ok", list);
        }
        
        /// <summary>
        /// 递归方式获取菜单操作列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>菜单树形结构</returns>
        public object GetMenuActionListRecursive(int roleId)
        {
            var menu = new List<MenuTree>();
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                menu = GetAllMenu()
                .Where(c => c.menu_type == 3 && c.delete_status == 0)
                .Select(x =>
                new MenuTree
                {
                    id = x.menu_id,
                    name = x.menu_name,
                    path = x.url,
                    parentId = x.parent_id,
                    component = x.component,
                    menuNameTW = x.menu_name_tw,
                    menuNameUS = x.menu_name_us,
                    icon = x.icon,
                    orderNo = x.orderNo,
                    hidden = x.hidden == 1 ? true : false,
                    parent_title = x.parent_title,
                    permission = x.Actions.Select(s => s.Value).ToArray()
                }).OrderBy(d => d.orderNo).ToList();
            }
            else
            {
                menu = (from a in UserContext.Current.Permissions
                        join b in GetAllMenu().Where(c => c.menu_type == 3 && c.delete_status == 0)
                        on a.Menu_Id equals b.menu_id
                        orderby b.orderNo descending
                        select new MenuTree
                        {
                            id = a.Menu_Id,
                            name = b.menu_name,
                            path = b.url,
                            parentId = b.parent_id,
                            component = b.component,
                            menuNameTW = b.menu_name_tw,
                            menuNameUS = b.menu_name_us,
                            icon = b.icon,
                            orderNo = b.orderNo,
                            hidden = b.hidden == 1 ? true : false,
                            parent_title = b.parent_title,
                            permission = a.UserAuthArr
                        }).OrderBy(d => d.orderNo).ToList();
            }

            // 使用递归方式构建菜单树
            List<MenuTree> rootMenus = BuildMenuTree(menu, 0);
            return WebResponseContent.Instance.OK("Ok", rootMenus);
        }

        /// <summary>
        /// 递归构建菜单树
        /// </summary>
        /// <param name="allMenus">所有菜单列表</param>
        /// <param name="parentId">父级ID</param>
        /// <returns>构建好的菜单树</returns>
        private List<MenuTree> BuildMenuTree(List<MenuTree> allMenus, int parentId)
        {
            List<MenuTree> result = new List<MenuTree>();
            
            // 获取当前父级下的所有子菜单
            var childMenus = allMenus.Where(m => m.parentId == parentId).ToList();
            
            foreach (var menu in childMenus)
            {
                MenuTree menuTree = new MenuTree
                {
                    id = menu.id,
                    name = menu.name,
                    path = menu.path,
                    component = menu.component,
                    menuNameUS = menu.menuNameUS,
                    menuNameTW = menu.menuNameTW,
                    icon = menu.icon,
                    permission = menu.permission,
                    hidden = menu.hidden,
                    parent_title = menu.parent_title
                };
                
                // 递归获取子菜单
                menuTree.children = BuildMenuTree(allMenus, menu.id);
                
                result.Add(menuTree);
            }
            
            return result;
        }

        public object GetMenuActionList(int roleId)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return GetAllMenu()
                .Where(c => c.menu_type == UserContext.MenuType)
                .Select(x =>
                new
                {
                    id = x.menu_id,
                    name = x.menu_name,
                    url = x.url,
                    parentId = x.parent_id,
                    icon = x.icon,
                    x.enable,
                    x.table_name, 
                    x.hidden,
                    x.parent_title,
                    permission = x.Actions.Select(s => s.Value).ToArray()
                }).ToList();
            }
            var menu = from a in UserContext.Current.Permissions
                       join b in GetAllMenu().Where(c => c.menu_type == UserContext.MenuType)
                       on a.Menu_Id equals b.menu_id
                       orderby b.orderNo descending
                       select new
                       {
                           id = a.Menu_Id,
                           name = b.menu_name,
                           url = b.url,
                           parentId = b.parent_id,
                           icon = b.icon,
                           b.enable,
                           b.table_name, // 2022.03.26增移动端加菜单类型
                           b.hidden,
                           b.parent_title,
                           permission = a.UserAuthArr
                       };
            return menu.ToList();
        }

        /// <summary>
        /// 新建或编辑菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Save(Sys_Menu menu)
        {
            WebResponseContent webResponse = new WebResponseContent();
            if (menu == null) return webResponse.Error("没有获取到提交的参数");
            if (menu.menu_id > 0 && menu.menu_id == menu.parent_id) return webResponse.Error("父级ID不能是当前菜单的ID");
            try
            {
                webResponse = menu.ValidationEntity(x => new { x.menu_name, x.table_name });
                if (!webResponse.status) return webResponse;
                if (menu.table_name != "/" && menu.table_name != ".")
                {
                    // 2022.03.26增移动端加菜单类型判断
                    Sys_Menu sysMenu = await _repository.FindAsyncFirst(x => x.table_name == menu.table_name);
                    if (sysMenu != null)
                    {
                        sysMenu.menu_type ??= 0;
                        if (sysMenu.menu_type == menu.menu_type)
                        {
                            if ((menu.menu_id > 0 && sysMenu.menu_id != menu.menu_id)
                            || menu.menu_id <= 0)
                            {
                                return webResponse.Error($"视图/表名【{menu.table_name}】已被其他菜单使用");
                            }
                        }
                    }
                }
                bool _changed = false;
                menu.menu_name_tw = ChineseConverter.Convert(menu.menu_name, ChineseConversionDirection.SimplifiedToTraditional);
                if (menu.menu_id <= 0)
                {
                    _repository.Add(menu.SetCreateDefaultVal());
                }
                else
                {
                    //2020.05.07新增禁止选择上级角色为自己
                    if (menu.menu_id == menu.parent_id)
                    {
                        return webResponse.Error($"父级id不能为自己");
                    }
                    if (_repository.Exists(x => x.parent_id == menu.menu_id && menu.parent_id == x.menu_id))
                    {
                        return webResponse.Error($"不能选择此父级id，选择的父级id与当前菜单形成依赖关系");
                    }

                    _changed = _repository.FindAsIQueryable(c => c.menu_id == menu.menu_id).Select(s => s.auth).FirstOrDefault() != menu.auth;

                    _repository.Update(menu.SetModifyDefaultVal(), p => new
                    {
                        p.parent_id,
                        p.menu_name,
                        p.url,
                        p.auth,
                        p.orderNo,
                        p.icon,
                        p.enable,
                        p.menu_type,// 2022.03.26增移动端加菜单类型
                        p.table_name,
                        p.hidden,
                        p.parent_title,
                        p.modify_date,
                        p.modify_name
                    });
                }
                await _repository.SaveChangesAsync();

                CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
                if (_changed)
                {
                    UserContext.Current.RefreshWithMenuActionChange(menu.menu_id);
                }
                _menus = null;
                webResponse.OK("保存成功", menu);
            }
            catch (Exception ex)
            {
                webResponse.Error(ex.Message + ex.StackTrace);
            }
            finally
            {
                Logger.Info($"表:{menu.table_name},菜单：{menu.menu_name},权限{menu.auth},{(webResponse.status ? "成功" : "失败")}{webResponse.message}");
            }
            return webResponse;

        }

        public async Task<WebResponseContent> DelMenu(int menuId)
        {
            WebResponseContent webResponse = new WebResponseContent();

            if (await _repository.ExistsAsync(x => x.parent_id == menuId))
            {
                return webResponse.Error("当前菜单存在子菜单,请先删除子菜单!");
            }
            _repository.Delete(new Sys_Menu()
            {
                menu_id = menuId
            }, true);
            CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
            return webResponse.OK("删除成功");
        }
        /// <summary>
        /// 编辑菜单时，获取菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<object> GetTreeItem(int menuId)
        {
            var sysMenu = (await _repository.FindAsync(x => x.menu_id == menuId))
                .Select(
                p => new
                {
                    p.menu_id,
                    p.parent_id,
                    p.menu_name,
                    p.url,
                    p.auth,
                    p.orderNo,
                    p.icon,
                    p.enable,
                    p.hidden,
                    p.parent_title,
                    // 2022.03.26增移动端加菜单类型
                    MenuType = p.menu_type ?? 0,
                    p.create_date,
                    p.create_name,
                    p.table_name,
                    p.modify_date
                }).FirstOrDefault();
            return sysMenu;
        }

        /// <summary>
        /// 设置菜单Hidden
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        public WebResponseContent SetMenuHidden(MenuHiddenDto menuDto)
        {
            try
            {
                if (menuDto.menu_id == 0) return WebResponseContent.Instance.Error("id_null");
                var entData = _repository.Find(d => d.menu_id == menuDto.menu_id).FirstOrDefault();
                if (entData != null)
                {
                    entData.hidden = menuDto.hidden == true ? byte.Parse("1") : byte.Parse("0");
                    entData.modify_date = DateTime.Now;
                    entData.modify_name = UserContext.Current.UserName;

                    var res = _repository.Update(entData, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), entData.menu_id);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        #region 2025-9-16
        public async Task<WebResponseContent> GetListByPage(PageInput<MenuQuery> query)
        {
            PageGridData<MenuListDto> pageGridData = new PageGridData<MenuListDto>();
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 && d.menu_type == 3 &&
            (queryPam.enable == -1 ? true : d.enable == queryPam.enable) &&
            (string.IsNullOrEmpty(queryPam.menu_name) ? true : d.menu_name.Contains(queryPam.menu_name))&& 
            (queryPam.parent_id ==-1 ?true: d.parent_id == queryPam.parent_id)).Select(d => new MenuListDto
            {
                menu_id = d.menu_id,
                menu_name = d.menu_name,
                menu_name_tw = d.menu_name_tw,
                menu_name_us = d.menu_name_us,
                icon = d.icon,
                url = d.url,
                //Actions = d.Actions,
                auth = d.auth,
                component = d.component,
                parent_id = d.parent_id,
                enable = d.enable,
                create_date = d.create_date,
                create_name = d.create_name,
                description = d.description,
                orderNo = d.orderNo,
                hidden = d.hidden == 1 ? true : false,
                parent_title = d.parent_title
            });
            query.sort_field = "orderNo";
            query.sort_type = "asc";
             var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }
        public async Task<WebResponseContent> GetParentMenu()
        {
            var data = (await _repository.FindAsync(d => d.parent_id == 0 && d.menu_type == 3 && d.delete_status == 0, a => new { a.menu_id, a.menu_name, a.menu_name_tw, a.menu_name_us })).ToList();
            return WebResponseContent.Instance.OK("Ok", data);
        }
        public async Task<WebResponseContent> GetMenuById(int id)
        {
            //var data = await _repository.FindFirstAsync(d => d.delete_status == 0 && d.menu_id == id);
            //if (data == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
            //else return WebResponseContent.Instance.OK(_localizationService.GetString("success"), data);

            var model = _repository.Find(d => d.menu_id == id).FirstOrDefault();
            if(model != null)
            {
                return WebResponseContent.Instance.OK(_localizationService.GetString("success"), new { 
                    menu_id = model.menu_id,
                    menu_name = model.menu_name,
                    menu_name_us = model.menu_name_us,
                    menu_name_tw = model.menu_name_tw,
                    auth = model.auth,
                    icon = model.icon,
                    component = model.component,
                    description = model.description,
                    enable = model.enable,
                    orderNo = model.orderNo,
                    table_name = model.table_name,
                    parent_id = model.parent_id,
                    url = model.url,
                    menu_type = model.menu_type,
                    delete_status = model.delete_status,
                    create_name = model.create_name,
                    create_date = model.create_date,
                    modify_name = model.modify_name,
                    modify_date = model.modify_date,
                    parent_title = model.parent_title,
                    hidden = model.hidden == 1 ? true : false
                });
            }

            return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
        }

        public WebResponseContent AddData(MenuAddDto menu) 
        {
            try
            {
                if (menu == null) return WebResponseContent.Instance.Error("content_connot_be_empty");

                if (string.IsNullOrEmpty(menu.menu_name))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("menu_name_null"));
                if (string.IsNullOrEmpty(menu.menu_name_us))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("menu_name_us_null"));
                if (string.IsNullOrEmpty(menu.menu_name_tw))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("menu_name_tw_null"));

                var model = _repository.FindAsIQueryable(x => x.delete_status == 0 && x.menu_name == menu.menu_name).FirstOrDefault();
                if (model != null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("existent"));
                }
                else
                {
                    Sys_Menu sys_Menu = new Sys_Menu();
                    sys_Menu.menu_name = menu.menu_name;
                    sys_Menu.menu_name_tw = menu.menu_name_tw;
                    sys_Menu.menu_name_us = menu.menu_name_us;
                    sys_Menu.parent_id = (int)menu.parent_id;
                    sys_Menu.url = menu.url;
                    sys_Menu.component = menu.component;
                    sys_Menu.icon = menu.icon;
                    sys_Menu.auth = menu.auth;
                    sys_Menu.orderNo = menu.orderNo;
                    sys_Menu.hidden = menu.hidden == true ? byte.Parse("1") : byte.Parse("0");
                    sys_Menu.parent_title = menu.parent_title;
                    sys_Menu.description = menu.description;
                    sys_Menu.enable = menu.enable;
                    sys_Menu.create_date = DateTime.Now;

                    sys_Menu.enable = 1;
                    sys_Menu.menu_type = 3;
                    sys_Menu.delete_status = 0;
                    sys_Menu.create_date = DateTime.Now;
                    sys_Menu.create_name = UserContext.Current.UserName;
                    _repository.Add(sys_Menu, true);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), sys_Menu.menu_id);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"{this.GetType().Name}_AddData异常：{ex.Message}");
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_AddData异常：{ex.Message}", ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent EditData(MenuDto menuDto) 
        {
            try
            {
                if (menuDto.menu_id == 0) return WebResponseContent.Instance.Error("id_null");
                var entData  = _repository.Find(d=>d.menu_id == menuDto.menu_id).FirstOrDefault();
                if (entData != null)
                {
                    entData.menu_name = menuDto.menu_name;
                    entData.menu_name_tw=menuDto.menu_name_tw;
                    entData.menu_name_us=menuDto.menu_name_us;
                    entData.parent_id = (int)menuDto.parent_id;
                    entData.url = menuDto.url;
                    entData.component = menuDto.component;
                    entData.icon = menuDto.icon;
                    entData.auth = menuDto.auth;
                    entData.orderNo = menuDto.orderNo;
                    entData.hidden = menuDto.hidden == true ? byte.Parse("1") : byte.Parse("0");
                    entData.parent_title = menuDto.parent_title;
                    entData.description = menuDto.description;
                    entData.enable = menuDto.enable;
                    entData.modify_date=DateTime.Now;
                    entData.modify_name = UserContext.Current.UserName;

                    var res = _repository.Update(entData,true);
                    if(res >0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), entData.menu_id);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                else 
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this,ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent DelData(int id) 
        {
            try
            {
                if (id == 0) return WebResponseContent.Instance.Error("id_null");
                var entModel = _repository.FindAsIQueryable(x => x.menu_id == id).FirstOrDefault();
                if (UserContext.Current.IsSuperAdmin)
                {
                    entModel.delete_status = (int)SystemDataStatus.Invalid;
                    entModel.modify_name = UserContext.Current.UserName;
                    entModel.modify_date = DateTime.Now;
                    var res = _repository.Update(entModel, true);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                }
                else
                {
                    //权限不足
                    return WebResponseContent.Instance.Error(_localizationService.GetString("insufficient_permissions"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this,ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        #endregion

        public async Task<WebResponseContent> GetMenuTree()
        {
            try
            {
                var list = await _repository.FindAsync(d => d.delete_status == (int)SystemDataStatus.Valid && d.menu_type ==3);
                var parent = list.Where(d => d.parent_id == 0);
                var children = list.Where(d => d.parent_id != 0);
                List<MenuTreeNode> lst = new List<MenuTreeNode>();
                foreach (var item in parent)
                {
                    MenuTreeNode treeNode = new MenuTreeNode();
                    treeNode.value = item.menu_id; 
                    treeNode.label = item.menu_name;
                    foreach (var child in children.Where(d => d.parent_id == item.menu_id))
                    {
                        MenuChildren children1 = new MenuChildren();
                        children1.value = child.menu_id; 
                        children1.label = child.menu_name;
                        treeNode.children.Add(children1);
                    }
                    lst.Add(treeNode);
                }
                //var json = treeNode.ToJson();
                return WebResponseContent.Instance.OK("Ok", lst);
            }
            catch (Exception ex)
            {
                return WebResponseContent.Instance.Error("No data");
            }

        }
    }
}


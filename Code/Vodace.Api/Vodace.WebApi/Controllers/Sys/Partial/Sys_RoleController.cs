using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.Controllers.Basic;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Filters;
using Vodace.Core.ManageUser;
using Vodace.Core.UserManager;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.AttributeManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Sys.Repositories;
using Vodace.Sys.Services;

namespace Vodace.Sys.Controllers
{
    //[Route("api/role")]
    public partial class Sys_RoleController
    {
        private readonly ISys_RoleService _service;//访问业务代码
        private readonly ISys_RoleRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_RoleController(
            ISys_RoleService service,
            ISys_RoleRepository repository,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet, Route("GetCurrentTreePermission")]
        [ApiActionPermission]
        public async Task<IActionResult> GetCurrentTreePermission()
        {
            return Json(await Service.GetCurrentTreePermission());
        }
        /// <summary>
        /// 获取菜单、页面、页面按钮（用于角色分配权限页面）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllTreePermission")]
        [ApiActionPermission]
        public async Task<IActionResult> GetALLTreePermission() 
        {
            return Json(await Service.GetALLTreePermission());
        }
        /// <summary>
        /// 获取角色分配的菜单、菜单按钮
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetCurrentTreePermissionByRoleId")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetCurrentTreePermission(int roleId)
        {
            return Json(await Service.GetCurrentTreePermission(roleId));
        }

        [HttpPost, Route("getUserTreePermission")]
        [ApiActionPermission]
        public async Task<IActionResult> GetUserTreePermission(int roleId)
        {
            return Json(await Service.GetUserTreePermission(roleId));
        }
        /// <summary>
        /// 保存角色 菜单权限
        /// </summary>
        /// <param name="userPermissions"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost, Route("savePermission")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission]
        //[ApiActionPermission(ActionPermissionOptions.Edit)]
        public async Task<IActionResult> SavePermission([FromBody] List<UserPermissions> userPermissions, int roleId)
        {
            return Json(await Service.SavePermission(userPermissions, roleId));
        }

        /// <summary>
        /// 获取当前角色下的所有角色 
        /// </summary>
        /// <returns></returns>

        [HttpPost, Route("getUserChildRoles")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public IActionResult GetUserChildRoles()
        {
            int roleId = UserContext.Current.RoleId;
            var data = RoleContext.GetAllChildren(UserContext.Current.RoleId);

            if (UserContext.Current.IsSuperAdmin)
            {
                return Json(WebResponseContent.Instance.OK(null, data));
            }
            //不是超级管理，将自己的角色查出来，在树形菜单上作为根节点
            var self = _repository.FindAsIQueryable(x => x.role_id == roleId)
                 .Select(s => new Vodace.Core.UserManager.RoleNodes()
                 {
                     Id = s.role_id,
                     RoleName = s.role_name
                 }).ToList();
            data.AddRange(self);
            return Json(WebResponseContent.Instance.OK(null, data));
        }



        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpPost, Route("GetPageData")]
        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return GetTreeTableRootData(loadData).Result;
        }

        /// <summary>
        /// treetable
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getTreeTableRootData")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetTreeTableRootData([FromBody] PageDataOptions options)
        {
            //页面加载根节点数据条件x => x.ParentId == 0,自己根据需要设置
            var query = _repository.FindAsIQueryable(x => x.delete_status ==0);
            if (UserContext.Current.IsSuperAdmin)
            {
                query = query.Where(x => true);
            }
            else
            {
                int roleId = UserContext.Current.RoleId;
                query = query.Where(x => x.role_id == roleId);
            }
            var queryChild = _repository.FindAsIQueryable(x => x.delete_status == 0);
            var rows = await query.TakeOrderByPage(options.Page, options.Rows)
                .OrderBy(x => x.role_id).Select(s => new
                {
                    s.role_id,
                    s.role_name,
                    s.remark,
                    s.enable,
                    s.create_date,
                    s.create_name,
                    s.modify_name,
                    s.modify_date,
                    s.order_no,
                }).ToListAsync();
            return JsonNormal(new { total = await query.CountAsync(), rows });
        }

        /// <summary>
        ///treetable 获取子节点数据(2021.05.02)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getTreeTableChildrenData")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetTreeTableChildrenData(int roleId)
        {
            if (!UserContext.Current.IsSuperAdmin && roleId != UserContext.Current.RoleId && !RoleContext.GetAllChildren(UserContext.Current.RoleId).Any(x => x.Id == roleId))
            {
                return JsonNormal(new { rows = new object[] { } });
            }
            //点击节点时，加载子节点数据
            var roleRepository = Sys_RoleRepository.Instance.FindAsIQueryable(x => true);
            var rows = await roleRepository.Select(s => new
            {
                s.role_id,
                s.role_name,
                s.remark,
                s.enable,
                s.create_date,
                s.create_name,
                s.modify_name,
                s.modify_date,
                s.order_no,
            }).ToListAsync();
            return JsonNormal(new { rows });
        }
        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost,Route("GetListByPage"),ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody]PageInput<RoleQuery> query) 
        {
            return Json(await _service.GetListByPage(query));
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet,Route("GetRoleById"),ApiActionPermission]
        public async Task<IActionResult> GetRoleById(int id) 
        {
            return Json(await _service.GetRoleById(id));
        }

        /// <summary>
        ///添加角色
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("AddData")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission(ActionPermissionOptions.Add)]
        public IActionResult AddData([FromBody] RoleDto role)
        {
            return Json(_service.AddData(role));
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Route("DelData")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission(ActionPermissionOptions.Delete)]
        public  IActionResult DelData(int role_id)
        {
            return Json(_service.DelData(role_id));
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("EditData")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission(ActionPermissionOptions.Update)]
        public IActionResult EditData([FromBody] EditRoleDto role)
        {
            return Json(_service.EditData(role));
        }
        [HttpPut,Route("SwitchEnable"),ApiActionPermission]
        public IActionResult SwitchEnable(int id, int enable) 
        {
            return Json(_service.SwitchEnable(id,enable));
        }
    }
}



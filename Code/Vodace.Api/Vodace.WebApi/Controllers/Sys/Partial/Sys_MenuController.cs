using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_MenuController
    {
        /// <summary_service
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost, Route("getTreeMenu")]
        [ApiExplorerSettings(IgnoreApi = true)]
        // [ApiActionPermission("Sys_Menu", ActionPermissionOptions.Search)]
        public IActionResult GetTreeMenu()
        {
            return Json( _service.GetCurrentMenuActionList());
        }
        [HttpGet, Route("GetCurrentMenuActionLisAll")]
        public IActionResult GetCurrentMenuActionLisAll() 
        {
            return Json(_service.GetCurrentMenuActionLisAll());
        }
        [HttpGet, HttpPost, Route("getTreeMenuNew")]
        public IActionResult GetTreeMenuNew() 
        {
            return Json(_service.GetCurrentMenuActionListNew());
        }
        [HttpPost, Route("getMenu")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission("Sys_Menu", ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetMenu()
        {
            return Json(await _service.GetMenu());
        }

        [HttpPost, Route("getTreeItem")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission("Sys_Menu", "1", ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetTreeItem(int menuId)
        {
            return Json(await _service.GetTreeItem(menuId));
        }

        //[ActionPermission("Sys_Menu", "1", ActionPermissionOptions.Add)]
        //只有角色ID为1的才能进行保存操作
        [HttpPost, Route("save"), ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Save([FromBody] Sys_Menu menu)
        {
            return Json(await _service.Save(menu));
        }

        /// <summary>
        /// 转换繁体
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpGet,Route("GetTraditional"),AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetTraditional(string str) 
        {
            return Json(ChineseConverter.Convert(str, ChineseConversionDirection.SimplifiedToTraditional));
        }

        /// <summary>
        /// 限制只能超级管理员才删除菜单 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost, Route("delMenu")]
        public async Task<ActionResult> DelMenu(int menuId)
        {
            return Json(await Service.DelMenu(menuId));
        }
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost,Route("GetListByPage"),ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody] PageInput<MenuQuery> query) 
        {
            return JsonNormal(await _service.GetListByPage(query));
        }
        /// <summary>
        /// 获取一级菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetParentMenu"), ApiActionPermission]
        public async Task<IActionResult>  GetParentMenu()
        {
            return Json(await _service.GetParentMenu());
        }
        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetMenuById"), ApiActionPermission]
        public async Task<IActionResult> GetMenuById(int id)
        {
            return Json(await _service.GetMenuById(id));
        }
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost, Route("AddData")]
        [ApiActionPermission(ActionPermissionOptions.Add)]
        public IActionResult AddData([FromBody] MenuAddDto menu) 
        {
            return Json(_service.AddData(menu));
        }
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPut, Route("EditData")]
        [ApiActionPermission(ActionPermissionOptions.Update)]
        public IActionResult EditData([FromBody] MenuDto menu) 
        { 
            return Json(_service.EditData(menu));
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelData")]
        [ApiActionPermission(ActionPermissionOptions.Delete)]
        public IActionResult DelData(int menuId) 
        {
            return Json(_service.DelData(menuId));
        }
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPut, Route("SetMenuHidden")]
        [ApiActionPermission(ActionPermissionOptions.Update)]
        public IActionResult SetMenuHidden([FromBody] MenuHiddenDto menu)
        {
            return Json(_service.SetMenuHidden(menu));
        }
        /// <summary>
        /// 获取1、2菜单树
        /// </summary>
        [HttpGet, Route("GetMenuTree"), ApiActionPermission]
        public async Task<IActionResult> GetMenuTree() 
        {
              return Json( await _service.GetMenuTree());
        }
    }
}

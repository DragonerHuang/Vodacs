
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Task_GroupController
    {
        private readonly IBiz_Task_GroupService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Task_GroupController(
            IBiz_Task_GroupService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        #region 操作方法
        /// <summary>
        /// 新增 Task AddTaskGroup
        /// </summary>
        /// <param name="mTaskAddTaskGroupDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddTaskGroup"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult AddTaskGroup([FromBody] TaskGroupDto dtoTaskAddTaskGroup)
        {
            return Json(_service.AddTaskGroup(dtoTaskAddTaskGroup));
        }

        /// <summary>
        /// 修改 Task Group
        /// </summary>
        /// <param name="mTaskGroupDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditTaskGroup"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult EditTask([FromBody] TaskGroupDto dtoTaskGroup)
        {
            return Json(_service.EditTaskGroup(dtoTaskGroup));
        }

        /// <summary>
        /// 删除 TaskGroup
        /// </summary>
        /// <param name="mTaskGroupDto"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelTaskGroup"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult DelTaskGroup(Guid guid)
        {
            return Json(_service.DelTaskGroup(guid));
        }

        /// <summary>
        /// 获取Task Group信息列表
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        [HttpPost, Route("GetTaskGroupList"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTasGroupkList([FromBody] PageDataOptions pdo)
        {
            return Json(await _service.GetTaskGroupList(pdo));
        }

        /// <summary>
        /// 获取Task Group信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //[HttpGet, Route("GetTaskById"), ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpGet, Route("GetTaskGroupById"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskGroupById(Guid guid)
        {
            return Json(await _service.GetTaskGroupById(guid));
        }
        #endregion
    }
}

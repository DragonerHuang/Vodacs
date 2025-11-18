
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
    public partial class Biz_Task_Group_RelationshipController
    {
        private readonly IBiz_Task_Group_RelationshipService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Task_Group_RelationshipController(
            IBiz_Task_Group_RelationshipService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        #region 操作方法
        /// <summary>
        /// 新增 Task Group Relationship
        /// </summary>
        /// <param name="mTaskAddTaskGroupDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddTaskGroup"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult AddTaskGroupRelationship([FromBody] TaskGroupRelationshipDto dtoTaskAddTaskGroupRelationship)
        {
            return Json(_service.AddTaskGroupRelationship(dtoTaskAddTaskGroupRelationship));
        }

        /// <summary>
        /// 修改 Task Group
        /// </summary>
        /// <param name="mTaskGroupDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditTaskGroupRelationship"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult EditTaskGroupRelationship([FromBody] TaskGroupRelationshipDto dtoTaskGroupRelationship)
        {
            return Json(_service.EditTaskGroupRelationship(dtoTaskGroupRelationship));
        }

        /// <summary>
        /// 删除 TaskGroup Relationship
        /// </summary>
        /// <param name="mTaskGroupDto"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelTaskGroupRelationship"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult DelTaskGroupRelationship(Guid guid)
        {
            return Json(_service.DelTaskGroupRelationship(guid));
        }

        /// <summary>
        /// 获取Task Group Relationship信息列表
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        [HttpPost, Route("GetTaskGroupRelationshipList"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTasGroupRelationshipList([FromBody] PageDataOptions pdo)
        {
            return Json(await _service.GetTaskGroupRelationshipList(pdo));
        }

        /// <summary>
        /// 获取Task Group Relationship信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //[HttpGet, Route("GetTaskGroupRelationshipById"), ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpGet, Route("GetTaskGroupRelationshipById"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskGroupRelationshipById(Guid guid)
        {
            return Json(await _service.GetTaskGroupRelationshipById(guid));
        }
        #endregion
    }
}

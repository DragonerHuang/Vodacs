
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Filters;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_TaskController
    {
        private readonly IBiz_TaskService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_TaskController(
            IBiz_TaskService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        #region 操作方法
        /// <summary>
        /// 新增 Task
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddTask"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult AddTask([FromBody] TaskDto dtoTask)
        {
            return Json(_service.AddTask(dtoTask));
        }

        /// <summary>
        /// 修改 Task
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditTask"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult EditTask([FromBody] TaskDto dtoTask)
        {
            return Json(_service.EditTask(dtoTask));
        }
        
        /// <summary>
        /// 删除 Task
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelTask"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult DelTask(Guid guid)
        {
            return Json(_service.DelTask(guid));
        }

        /// <summary>
        /// 获取Task信息列表
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        [HttpPost, Route("GetTaskList"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskList([FromBody] PageDataOptions pdo)
        {
            return Json(await _service.GetTaskList(pdo));
        }

        /// <summary>
        /// 获取Task信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //[HttpGet, Route("GetTaskById"), ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpGet, Route("GetTaskById"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskById(Guid guid)
        {
            return Json(await _service.GetTaskById(guid));
        }
        #endregion
    }
}

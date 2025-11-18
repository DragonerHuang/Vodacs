
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
    

    public partial class Biz_Task_DetailController
    {
        private readonly IBiz_Task_DetailService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Task_DetailController(
            IBiz_Task_DetailService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        #region 操作方法
        /// <summary>
        /// 新增 TaskDetail
        /// </summary>
        /// <param name="mTaskDetailDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddTaskDetail"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult AddTaskDetail([FromBody] TaskDetailDto dtoTaskDetail)
        {
            return Json(_service.AddTaskDetail(dtoTaskDetail));
        }

        /// <summary>
        /// 修改 TaskDetail
        /// </summary>
        /// <param name="mTaskDetailDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditTaskDetail"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult EditTaskDetail([FromBody] TaskDetailDto dtoTaskDetail)
        {
            return Json(_service.EditTaskDetail(dtoTaskDetail));
        }

        /// <summary>
        /// 删除 TaskDetail
        /// </summary>
        /// <param name="mTaskDetailDto"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelTaskDetail"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult DelTaskDetail(Guid guid)
        {
            return Json(_service.DelTaskDetail(guid));
        }

        /// <summary>
        /// 获取TaskDetail信息列表
        /// </summary>
        /// <param name="mTaskDetailDto"></param>
        /// <returns></returns>
        [HttpPost, Route("GetTaskDetailList"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDetailList([FromBody] PageDataOptions pdo)
        {
            return Json(await _service.GetTaskDetailList(pdo));
        }

        /// <summary>
        /// 获取TaskDetail信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //[HttpGet, Route("GetTaskDetailById"), ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpGet, Route("GetTaskDetailById"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDetailById(Guid guid)
        {
            return Json(await _service.GetTaskDetailById(guid));
        }
        #endregion

    }
}

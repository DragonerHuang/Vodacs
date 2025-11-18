
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
    public partial class Biz_Task_Detail_Work_TypeController
    {
        private readonly IBiz_Task_Detail_Work_TypeService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Task_Detail_Work_TypeController(
            IBiz_Task_Detail_Work_TypeService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        #region 操作方法
        /// <summary>
        /// 新增 Task Work Type
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddTaskWorkType"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult AddTaskWorkType([FromBody] TaskWorkTypeDto dtoTaskWorkType)
        {
            return Json(_service.AddTaskWorkType(dtoTaskWorkType));
        }

        /// <summary>
        /// 修改 TaskWorkType
        /// </summary>
        /// <param name="mTaskWorkTypeDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditTaskWorkType"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult EditTaskWorkType([FromBody] TaskWorkTypeDto dtoTaskWorkType)
        {
            return Json(_service.EditTaskWorkType(dtoTaskWorkType));
        }

        /// <summary>
        /// 删除 TaskWorkType
        /// </summary>
        /// <param name="mTaskWorkTypeDto"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelTaskWorkType"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public IActionResult DelTaskWorkType(Guid guid)
        {
            return Json(_service.DelTaskWorkType(guid));
        }

        ///// <summary>
        ///// 获取TaskWorkType信息列表
        ///// </summary>
        ///// <param name="mTaskWorkTypeDto"></param>
        ///// <returns></returns>
        //[HttpPost, Route("GetTaskWorkTypeList"), AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        ////[ApiActionPermission]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetTaskWorkTypeList([FromBody] PageDataOptions pdo)
        //{
        //    return Json(await _service.GetTaskWorkTypeList(pdo));
        //}

        /// <summary>
        /// 获取TaskWorkType信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //[HttpGet, Route("GetTaskWorkTypeById"), ApiActionPermission(ActionPermissionOptions.Search)]
        [HttpGet, Route("GetTaskWorkTypeById"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        //[ApiActionPermission]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskWorkTypeById(Guid guid)
        {
            return Json(await _service.GetTaskWorkTypeById(guid));
        }
        #endregion
    }
}

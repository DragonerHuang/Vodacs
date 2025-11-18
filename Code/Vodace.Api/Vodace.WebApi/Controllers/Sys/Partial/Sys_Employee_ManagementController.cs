using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    /// <summary>
    /// 考勤管理-员工管理
    /// </summary>
    [ApiActionPermission]
    public partial class Sys_Employee_ManagementController
    {
        private readonly ISys_Employee_ManagementService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Employee_ManagementController(
            ISys_Employee_ManagementService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取员工列表-分页
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetListByPage")]
        public async Task<IActionResult> GetListByPageAsync([FromBody] PageInput<SysEmpMentQueryDto> pageInput)
        {
            return Json(await _service.GetListByPageAsync(pageInput));
        }

        /// <summary>
        /// 获取员工列表
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateOrUpdateData")]
        public async Task<IActionResult> CreateOrUpdateDataAsync([FromForm] SysEmpMentCreateOrUpdateDto addDto)
        {
            return Json(await _service.CreateOrUpdateAsync(addDto));
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelData")]
        public async Task<IActionResult> DelData(List<Guid> ids)
        {
            return Json(await _service.DelAsync(ids));
        }

        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetForEdit")]
        public async Task<IActionResult> GetForEditAsync(Guid id)
        {
            return Json(await _service.GetForEditAsync(id));
        }
    }
}
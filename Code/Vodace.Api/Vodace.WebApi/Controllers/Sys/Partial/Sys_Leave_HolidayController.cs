using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Vodace.Core.Extensions;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_Leave_HolidayController
    {
        private readonly ISys_Leave_HolidayService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_HolidayController(
            ISys_Leave_HolidayService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取公共假期列表-分页
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetListByPage"), ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody] PageInput<LeaveHolidayQuery> pageInput)
        {
            return Json(await _service.GetListByPage(pageInput));
        }

        /// <summary>
        /// 获取公共假期列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetList"), ApiActionPermission]
        public async Task<IActionResult> GetList()
        {
            return Json(await _service.GetList());
        }

        /// <summary>
        /// 添加公共假期
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddData"), ApiActionPermission]
        public async Task<IActionResult> AddData([FromBody] LeaveHolidayAddDto addDto)
        {
            return Json(await _service.AddData(addDto));
        }

        /// <summary>
        /// 修改公共假期
        /// </summary>
        /// <param name="editDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditData"), ApiActionPermission]
        public async Task<IActionResult> EdidData([FromBody] LeaveHolidayEditDto editDto)
        {
            return Json(await _service.EditData(editDto));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelData"), ApiActionPermission]
        public async Task<IActionResult> DelData(List<Guid> id)
        {
            return Json(await _service.DelData(id));
        }
        
        /// <summary>
        /// json导入日期
        /// </summary>
        /// <param name="importDto"></param>
        /// <returns></returns>
        [HttpPost, Route("ImportData"), ApiActionPermission]
        public async Task<IActionResult> ImportData([FromBody] LeaveHolidayImportDto importDto)
        {
            return Json(await _service.ImportDataAsync(importDto));
        }
        
    }
}
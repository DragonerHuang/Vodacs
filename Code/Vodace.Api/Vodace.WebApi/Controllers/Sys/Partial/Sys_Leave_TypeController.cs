
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;
using Vodace.Core.Filters;
using Vodace.Entity;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_Leave_TypeController
    {
        private readonly ISys_Leave_TypeService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Leave_TypeController(
            ISys_Leave_TypeService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 获取假期类型列表-分页
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        [HttpPost,Route("GetListByPage"), ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody]PageInput<LeaveTypeQuery> pageInput)
        {
            return Json(await _service.GetListByPage(pageInput));
        }
        /// <summary>
        /// 获取假期类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetList"), ApiActionPermission]
        public async Task<IActionResult> GetList()
        {
            return Json(await _service.GetList());
        }
        /// <summary>
        /// 添加假期类型
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        [HttpPost,Route("AddData"), ApiActionPermission]
        public async Task<IActionResult> AddData([FromBody] LeaveTypeAddDto addDto) 
        {
            return Json(await _service.Add(addDto));
        }
        /// <summary>
        /// 修改假期类型
        /// </summary>
        /// <param name="editDto"></param>
        /// <returns></returns>
        [HttpPut,Route("EditData"), ApiActionPermission]
        public async Task<IActionResult> EdidData([FromBody]LeaveTypeEditDto editDto) 
        {
            return Json(await _service.Update(editDto));
        }
        /// <summary>
        /// 删除假期类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete,Route("DelData"), ApiActionPermission]
        public async Task<IActionResult> DelData(Guid id)
        {
            return Json(await _service.DelData(id));
        }
    }
}

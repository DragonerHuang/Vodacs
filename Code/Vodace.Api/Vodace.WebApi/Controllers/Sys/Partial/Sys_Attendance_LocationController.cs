
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_Attendance_LocationController
    {
        private readonly ISys_Attendance_LocationService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Attendance_LocationController(
            ISys_Attendance_LocationService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取列表（分页）
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetLocationPageData")]
        [ApiActionPermission]
        public async Task<IActionResult> GetPageData([FromBody] PageInput<LocationSearchDto> dtoSearchInput)
        {
            var result = await _service.GetDataListAsync(dtoSearchInput);
            return Json(result);
        }

        /// <summary>
        /// 获取列表（不分页）
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetLocationData")]
        [ApiActionPermission]
        public async Task<IActionResult> GetData([FromBody] LocationSearchDto dtoSearchInput)
        {
            var result = await _service.GetDataListAsync(dtoSearchInput);
            return Json(result);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        [HttpPost, Route("AddLocation")]
        [ApiActionPermission]
        public async Task<IActionResult> AddData([FromBody] LocationInputDto dtoInput)
        {
            var result =await _service.AddData(dtoInput);

            return Json(result);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="dtoEdit"></param>
        /// <returns></returns>
        [HttpPut, Route("EditLocation")]
        [ApiActionPermission]
        public async Task<IActionResult> EditData([FromBody] LocationEditDto dtoEdit)
        {
            var result = await _service.EditData(dtoEdit);

            return Json(result);
        }

        /// <summary>
        /// 删除数据（软删）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteLocation")]
        [ApiActionPermission]
        public async Task<IActionResult> DeleteData(Guid id)
        {
            var result = await _service.DeleteData(id);

            return Json(result);
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Contract_DetailsController
    {
        private readonly IBiz_Contract_DetailsService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Contract_DetailsController(
            IBiz_Contract_DetailsService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// 获取合同资料
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetContractData")]
        [ApiVersion("1.0")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContractData(Guid qnId)
        {
            var result = await _service.GetContractData(qnId);
            return Json(result);
        }

        /// <summary>
        /// 保存合同资料
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("SaveContractData")]
        [ApiVersion("1.0")]
        [ApiActionPermission]
        public async Task<IActionResult> SaveContractData([FromBody] ContractDetailDataDto dtoInput)
        {
            var result = await _service.SaveContractData(dtoInput);
            return Json(result);
        }

        /// <summary>
        /// 投标补充-附件文档上传
        /// </summary>
        /// <param name="litFiles"></param>
        /// <returns></returns>
        [HttpPost, Route("UploadAddendumFiles")]
        [ApiActionPermission]
        public async Task<IActionResult> UploadAddendumFiles(List<IFormFile> litFiles)
        {
            var result = await _service.UploadAddendumFiles(litFiles);
            return Json(result);
        }

        /// <summary>
        /// 获取标书资料
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetTenderData")]
        [ApiVersion("1.0")]
        [ApiActionPermission]
        public async Task<IActionResult> GetTenderData(Guid qnId)
        {
            var result = await _service.GetTenderData(qnId);
            return Json(result);
        }

        /// <summary>
        /// 保存标书资料
        /// </summary>
        /// <returns></returns>
        [HttpPut, Route("SaveTenderData")]
        [ApiVersion("1.0")]
        [ApiActionPermission]
        public async Task<IActionResult> SaveTenderData([FromBody] TenderDataDto dtoInput)
        {
            var result = await _service.SaveTenderData(dtoInput);
            return Json(result);
        }
    }



}

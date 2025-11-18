
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
    public partial class Biz_ContractOrgController
    {
        private readonly IBiz_ContractOrgService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_ContractOrgController(
            IBiz_ContractOrgService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost, Route("AddContractOrg")]
        [ApiActionPermission]
        public IActionResult AddContractOrg([FromBody] ContractOrgDto dto)
        {
            return Json(_service.Add(dto));
        }
        [HttpPost, Route("AddBatchContractOrg")]
        [ApiActionPermission]
        public IActionResult AddBatchContractOrg([FromBody] List<ContractOrgDto> dto)
        {
            return Json(_service.AddBatch(dto));
        }

        [HttpDelete, Route("DelContractOrg")]
        [ApiActionPermission]
        public IActionResult DelContractOrg(Guid guid)
        {
            return Json(_service.Del(guid));
        }

        [HttpPut, Route("EditContractOrg")]
        [ApiActionPermission]
        public IActionResult EditContractOrg([FromBody] ContractOrgDto dto)
        {
            return Json(_service.Edit(dto));
        }

        [HttpPost, Route("GetContractOrgList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContractOrgList([FromBody] PageInput<ContractOrgSearchDto> dto)
        {
            return Json(await _service.GetContractOrgList(dto));
        }
    }
}

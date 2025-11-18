
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_Site_Work_Record_SignController
    {
        private readonly IBiz_Site_Work_Record_SignService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Site_Work_Record_SignController(
            IBiz_Site_Work_Record_SignService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}

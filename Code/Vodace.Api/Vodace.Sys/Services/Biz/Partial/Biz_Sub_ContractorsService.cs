
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Sub_ContractorsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Sub_ContractorsRepository _repository;//访问数据库

        private readonly ILocalizationService _localizationService; // 国际化

        [ActivatorUtilitiesConstructor]
        public Biz_Sub_ContractorsService(
            IBiz_Sub_ContractorsRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 获取合约客户下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContractorsDownList()
        {
            try
            {
                var lstContractores = await _repository.FindAsync(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstDownData = new List<ContractorsDownDto>();
                foreach (var item in lstContractores)
                {
                    lstDownData.Add(new ContractorsDownDto
                    {
                        id = item.id,
                        name = item.name_sho
                    });
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstDownData);
                //ContractorsDownDto
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
  }
}

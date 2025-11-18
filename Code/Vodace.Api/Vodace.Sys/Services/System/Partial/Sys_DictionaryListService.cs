using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Sys_DictionaryListService
    {

        private readonly ISys_DictionaryRepository _sysDictionaryRepository;            //访问数据字典数据库
        private readonly ILocalizationService _localizationService;                     //国际化
        private readonly ISys_DictionaryListRepository _sysDicListRepository;           //访问数据字典子表

        public Sys_DictionaryListService(
            ISys_DictionaryRepository sysDictionaryRepository,
            ILocalizationService localizationService,
            ISys_DictionaryListRepository sysDicListRepository)
        {
            _sysDictionaryRepository = sysDictionaryRepository;
            _localizationService = localizationService;
            _sysDicListRepository = sysDicListRepository;
        }

        public override PageGridData<Sys_Dictionary_List> GetPageData(PageDataOptions pageData)
        {
            base.OrderByExpression = x => new Dictionary<object, QueryOrderBy>() { {
                    x.order_no,QueryOrderBy.Desc
                },
                {
                    x.dic_list_id,QueryOrderBy.Asc
                }
            };
            return base.GetPageData(pageData);
        }

        /// <summary>
        /// 根据code获取字典列表
        /// </summary>
        /// <param name="dicCode"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDictionaryByCode(string dicCode)
        {
            try
            {
                var lstMainDic = await _sysDictionaryRepository.FindFirstAsync(p => p.dic_no == dicCode && p.delete_status == 0);
                if (lstMainDic == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("dic_null"));
                }

                var lstSubDic = await _sysDicListRepository
                    .WhereIF(true, p => p.dic_id == lstMainDic.dic_id && p.delete_status == 0)
                    .OrderBy(p => p.order_no)
                    .ToListAsync();
                var lstStutsDto = new List<ContractStatusDto>();
                foreach (var item in lstSubDic)
                {
                    lstStutsDto.Add(new ContractStatusDto
                    {
                        id = item.dic_list_id,
                        index = item.order_no,
                        code = item.dic_value,
                        value = item.dic_name,
                        value_eng = item.dic_name_eng,
                    });
                }
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstStutsDto);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}


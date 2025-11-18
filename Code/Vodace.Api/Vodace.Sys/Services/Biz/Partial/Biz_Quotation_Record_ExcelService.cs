
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using Vodace.Core.BaseProvider;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_Record_ExcelService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Quotation_Record_ExcelRepository _repository;//访问数据库
        private readonly IBiz_Quotation_RecordRepository _quotationRecordRepository;//访问数据库
        private readonly IBiz_QuotationRepository _quotationRepository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_Record_ExcelService(
            IBiz_Quotation_Record_ExcelRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IMapper mapper,
            IBiz_QuotationRepository quotationRepository,
            IBiz_Quotation_RecordRepository quotationRecordRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            _quotationRepository = quotationRepository;
            _quotationRecordRepository = quotationRecordRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public WebResponseContent Add(QuotationRecordExcelDto dtoQuotationRecordExcel)
        {
            try
            {
                if (dtoQuotationRecordExcel == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                Biz_Quotation_Record_Excel biz_Quotation_Record_Excel = _mapper.Map<Biz_Quotation_Record_Excel>(dtoQuotationRecordExcel);
                biz_Quotation_Record_Excel.id = Guid.NewGuid();
                biz_Quotation_Record_Excel.delete_status = (int)SystemDataStatus.Valid;
                biz_Quotation_Record_Excel.create_id = UserContext.Current.UserId;
                biz_Quotation_Record_Excel.create_name = UserContext.Current.UserName;
                biz_Quotation_Record_Excel.create_date = DateTime.Now;
                biz_Quotation_Record_Excel.line_number = GetQRExcelLineNumber((Guid)dtoQuotationRecordExcel.quotation_record_id, dtoQuotationRecordExcel.sheet_name);
                _repository.Add(biz_Quotation_Record_Excel, true);
                return WebResponseContent.Instance.OK("Ok", biz_Quotation_Record_Excel);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_Record_ExcelService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                Biz_Quotation_Record_Excel biz_Quotation_Record_Excel = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (biz_Quotation_Record_Excel != null)
                {
                    biz_Quotation_Record_Excel.delete_status = (int)SystemDataStatus.Invalid;
                    biz_Quotation_Record_Excel.modify_id = UserContext.Current.UserId;
                    biz_Quotation_Record_Excel.modify_name = UserContext.Current.UserName;
                    biz_Quotation_Record_Excel.modify_date = DateTime.Now;
                    _repository.Update(biz_Quotation_Record_Excel, true);

                    return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_Record_ExcelService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(QuotationRecordExcelEditDto dtoQuotationRecordExcel)
        {
            try
            {
                if (dtoQuotationRecordExcel.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                Biz_Quotation_Record_Excel biz_Quotation_Record_Excel = _repository.Find(p => p.id == dtoQuotationRecordExcel.id && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();                
                if (biz_Quotation_Record_Excel != null)
                {
                    //biz_Quotation_Record_Excel.quotation_record_id = dtoQuotationRecordExcel.quotation_record_id;//这个不作修改
                    //biz_Quotation_Record_Excel.sheet_name = dtoQuotationRecordExcel.sheet_name;//这个不作修改
                    //biz_Quotation_Record_Excel.line_number = dtoQuotationRecordExcel.line_number;//这个不作修改
                    
                    biz_Quotation_Record_Excel.item_code = dtoQuotationRecordExcel.item_code;
                    biz_Quotation_Record_Excel.item_description = dtoQuotationRecordExcel.item_description;
                    biz_Quotation_Record_Excel.quantity = dtoQuotationRecordExcel.quantity;
                    biz_Quotation_Record_Excel.unit = dtoQuotationRecordExcel.unit;
                    biz_Quotation_Record_Excel.unit_rage = dtoQuotationRecordExcel.unit_rage;
                    biz_Quotation_Record_Excel.amount = dtoQuotationRecordExcel.amount;

                    //biz_Quotation_Record_Excel.delete_status = (int)SystemDataStatus.Valid;
                    biz_Quotation_Record_Excel.modify_id = UserContext.Current.UserId;
                    biz_Quotation_Record_Excel.modify_name = UserContext.Current.UserName;
                    biz_Quotation_Record_Excel.modify_date = DateTime.Now;
                    _repository.Update(biz_Quotation_Record_Excel, true);

                    return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"), biz_Quotation_Record_Excel);
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{dtoQuotationRecordExcel.id}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_Record_ExcelService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> GetQuotationRecordExcelList(PageInput<SearchQuotationRecordExcelInputDto> dtoSearchInput)
        {
            try
            {
                var qnSearch = dtoSearchInput?.search;

                var lstRecord = _quotationRecordRepository.Find(a => a.id == qnSearch.quotation_record_id)
                    .Select(a => new { a.id, a.qn_id, a.version, a.amount, a.file_name })
                    .FirstOrDefault();
                if (lstRecord == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent"));
                }

                // 使用 LINQ 一次性获取所需数据，减少多次数据库查询
                var qnInfo = _quotationRepository.Find(a => a.id == lstRecord.qn_id).Select(a => new { a.id, a.qn_no }).FirstOrDefault();
                if (qnInfo == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("quotation_null"));
                }

                // 获取有效数据并按 sheet_name 分组和排序
                var lstExcel = _repository.Find(a => a.quotation_record_id == lstRecord.id && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                if (!lstExcel.Any())
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent"));
                }

                // 使用 LINQ 进行分组、排序和映射，减少嵌套循环
                var lstResult = lstExcel
                    .GroupBy(a => a.sheet_name)
                    .OrderBy(g => g.Key)
                    .Select(group => new QuotationRecordExcelDetailListDto
                    {
                        sheet_name = group.Key,
                        list = group.Select(r => new QuotationRecordExcelListDto
                        {
                            version = lstRecord.version,
                            amount = r.amount?.ToString() ?? string.Empty,
                            file_name = lstRecord.file_name,
                            qn_no = qnInfo.qn_no,
                            delete_status = r.delete_status,
                            remark = r.remark,
                            create_id = r.create_id,
                            create_name = r.create_name,
                            create_date = r.create_date,
                            modify_id = r.modify_id,
                            modify_name = r.modify_name,
                            modify_date = r.modify_date,
                            id = r.id,
                            quotation_record_id = r.quotation_record_id,
                            sheet_name = r.sheet_name,
                            item_code = r.item_code,
                            item_description = r.item_description,
                            quantity = r.quantity,
                            unit = r.unit,
                            unit_rage = r.unit_rage,
                            line_number = r.line_number
                        }).OrderBy(a => a.line_number).ToList()
                    }).ToList();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstResult);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_Record_ExcelService.GetQuotationRecordList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public int GetQRExcelLineNumber(Guid intQuotationRecordId, string strSheetName)
        {
            int res = 0;
            try
            {
                var lastModel = _repository
                    .Find(a => a.quotation_record_id == intQuotationRecordId && a.sheet_name == strSheetName && a.delete_status == (int)SystemDataStatus.Valid)
                    .Select(a => new { a.line_number })
                    .OrderByDescending(a => a.line_number)
                    .FirstOrDefault();
                if (lastModel != null)
                    res = (int)(lastModel.line_number) + 1;
                else
                    res = 1;
            }
            catch(Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_Record_ExcelService.GetQRExcelLineNumber", e);
            }
            return res;
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.Configuration;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Site_Work_Record_Item_CheckService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Site_Work_Record_Item_CheckRepository _repository;//访问数据库

        private readonly ISys_Site_Work_Check_ItemRepository _sysCheckItemRepository; // 选项仓储
        private readonly IBiz_Site_Work_RecordRepository _siteWorkRecordRepository;   // 工地工作记录仓储
        private readonly IBiz_Site_Work_Record_SignRepository _signRepository;        // 签名仓储
        private readonly ISys_ContactRepository _contactRepository;                   // 联系人仓储
        private readonly IBiz_ContractRepository _contractRepository;                 // 合同仓储
        private readonly IBiz_Site_Work_Record_WorkerRepository _workerRepository;    // 工人仓储
        private readonly IBiz_SiteRepository _siteRepository;                         // 地点仓储

        private readonly ILocalizationService _localizationService;                   // 国际化服务
        private readonly IBiz_Project_FilesService _projectfilesService;              // 项目文件服务
        private readonly ISys_File_RecordsService _sysFileRecordsService;             // 文件记录服务（系统）

        [ActivatorUtilitiesConstructor]
        public Biz_Site_Work_Record_Item_CheckService(
            IBiz_Site_Work_Record_Item_CheckRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ISys_Site_Work_Check_ItemRepository sysCheckItemRepository,
            IBiz_Site_Work_Record_SignRepository siteWorkRecordSignRepository,
            ISys_ContactRepository contactRepository,
            IBiz_Site_Work_RecordRepository siteWorkRecordRepository,
            IBiz_ContractRepository contractRepository,
            IBiz_Project_FilesService projectfilesService,
            ISys_File_RecordsService sysFileRecordsService,
            IBiz_Site_Work_Record_WorkerRepository workerRepository,
            IBiz_SiteRepository siteRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _sysCheckItemRepository = sysCheckItemRepository;
            _signRepository = siteWorkRecordSignRepository;
            _contactRepository = contactRepository;
            _siteWorkRecordRepository = siteWorkRecordRepository;
            _contractRepository = contractRepository;
            _projectfilesService = projectfilesService;
            _sysFileRecordsService = sysFileRecordsService;
            _workerRepository = workerRepository;
            _siteRepository = siteRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 通用分发接口，根据 checkType 返回对应结构
        /// 0:BRF, 1:SCR, 2:CPD, 3:QDC, 4:CP, 5:SIC
        /// </summary>
        public async Task<WebResponseContent> GetCheckItemsAsync(Guid recordId, int checkType)
        {
            var lstItems = await GetCheckItemByRecordIdAsync(recordId, checkType);
            var lstItemValues = await _repository
                   .FindAsIQueryable(v => v.check_type == checkType && v.record_id == recordId)
                   .AsNoTracking()
                   .ToListAsync();
            switch (checkType)
            {
                case 0:
                    return GetBrfAsync(recordId, lstItems, lstItemValues);
                case 1:
                    return await GetScrAsync(recordId, lstItems, lstItemValues);
                case 2:
                    return await GetCpdAsync(recordId, lstItems, lstItemValues);
                case 3:
                    return await GetQdcAsync(recordId, lstItems, lstItemValues);
                case 4:
                    return await GetCpAsync(recordId, lstItems, lstItemValues);
                case 5:
                    return await GetSicAsync(recordId, lstItems, lstItemValues);
                default:
                    return WebResponseContent.Instance.Error("Invalid checkType. Use 0(BRF),1(SCR),2(CPD),3(QDC),4(CP),5(SIC)");
            }
        }

        #region 获取列表

        /// <summary>
        /// 安全简介记录 BRF （Briefing Record）
        /// 单独接口：两层结构，第二层为准（选择框+文本框）
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="lstItems"></param>
        /// <param name="lstItemValues"></param>
        /// <returns></returns>
        public WebResponseContent GetBrfAsync(
            Guid recordId, 
            List<Sys_Site_Work_Check_Item> lstItems,
            List<Biz_Site_Work_Record_Item_Check> lstItemValues)
        {
            try
            {
                var allItems = lstItems.Where(i => i.enable == 1 && 
                                                   !string.IsNullOrEmpty(i.global_code) && 
                                                   i.global_code.StartsWith("B"))
                    .Select(i => new { i.level, i.item_code, i.master_id, i.global_code, i.name_cht, i.name_eng, i.order_no })
                    .ToList();

                var result = new List<BrfItemDto>();
                var lvl1 = allItems.Where(i => i.level == 1)
                                   .OrderBy(p => p.order_no)
                                   .ToList();
                var lvl2 = allItems.Where(i => i.level == 2)
                                   .OrderBy(x => x.global_code)
                                   .ToList();

                foreach (var l1 in lvl1)
                {
                    var twoLvl = lvl2.Where(p => p.master_id == l1.item_code).OrderBy(p => p.order_no).ToList();
                    foreach (var l2 in twoLvl)
                    {
                        var val = lstItemValues.FirstOrDefault(v => v.check_code == l2.item_code);
                        result.Add(new BrfItemDto
                        {
                            level1_code = l1.item_code,
                            level1_name_cht = l1.name_cht,
                            level1_name_eng = l1.name_eng,
                            level1_index = l1.order_no ?? 0,
                            level2_code = l2.item_code,
                            level2_name_cht = l2.name_cht,
                            level2_name_eng = l2.name_eng,
                            level2_index = l2.order_no ?? 0,
                            global_code = l2.global_code,
                            value = MakeValue(val)
                        });
                    }
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 每日安全记录  SCR （Safety Working Cycle） 
        /// 单独接口：1/2/3 层结构（选择框）
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="lstItems"></param>
        /// <param name="lstItemValues"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetScrAsync(
            Guid recordId, 
            List<Sys_Site_Work_Check_Item> lstItems,
            List<Biz_Site_Work_Record_Item_Check> lstItemValues)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;
                var scrPrefixes = new[] { "S101", "S102", "S103", "S104", "S105", "S106", "S107" };
                bool HasPrefix(string code) => !string.IsNullOrEmpty(code) && scrPrefixes.Any(p => code.StartsWith(p));

                var scrVals = lstItemValues;
                var l1s = lstItems.Where(i => i.level == 1 && HasPrefix(i.global_code)).OrderBy(i => i.order_no).ToList();
                var l2s = lstItems.Where(i => i.level == 2 && HasPrefix(i.global_code)).ToList();
                var l3s = lstItems.Where(i => i.level == 3 && HasPrefix(i.global_code)).ToList();

                var output = new List<ScrSectionDto>();
                foreach (var t in l1s)
                {
                    var section = new ScrSectionDto
                    {
                        title_code = t.item_code,
                        title_cht = t.name_cht,
                        title_eng = t.name_eng,
                        global_code = t.global_code
                    };

                    // 当前 Section 类型前缀（S101/S102/S103/S104/S105/S106）
                    var secPrefix = scrPrefixes.FirstOrDefault(p => !string.IsNullOrEmpty(t.global_code) && t.global_code.StartsWith(p));

                    var rows2 = l2s.Where(x => x.master_id == t.item_code).OrderBy(p => p.order_no).ToList();
                    foreach (var c2 in rows2)
                    {
                        var row = new ScrLevel2Dto
                        {
                            code = c2.item_code,
                            name_cht = c2.name_cht,
                            name_eng = c2.name_eng,
                            global_code = c2.global_code
                        };

                        // 对应行的值记录（check_type=1，check_code=二层item_code）
                        var val2 = scrVals.FirstOrDefault(v => v.check_code == c2.item_code);

                        if (secPrefix == "S101" || secPrefix == "S102" || secPrefix == "S107")
                        {
                            var children3 = l3s.Where(x => x.master_id == c2.item_code).OrderBy(p => p.order_no).ToList();
                            foreach (var c3 in children3)
                            {
                                var val = scrVals.FirstOrDefault(v => v.check_code == c3.item_code);
                                row.children.Add(new ScrLevel3Dto
                                {
                                    code = c3.item_code,
                                    name_cht = c3.name_cht,
                                    name_eng = c3.name_eng,
                                    global_code = c3.global_code,
                                    value = MakeValue(val)
                                });
                            }
                        }
                        else
                        {
                            row.value = MakeValue(val2);
                        }
                       
                        section.items.Add(row);
                    }
                    output.Add(section);
                }

                // 获取签名
                var lstSign = await GetSignAsync(recordId, new List<int?> { (int)SignTypeEnum.scr_spq, (int)SignTypeEnum.scr_wpic });

                var result = new SafetyWorkingCycleDto
                {
                    safety_morning_meeting = output.FirstOrDefault(p => p.global_code == "S101"),
                    pre_work_check = output.FirstOrDefault(p => p.global_code == "S102"),
                    safety_record = output.FirstOrDefault(p => p.global_code == "S103"),
                    guidance_supervision = output.FirstOrDefault(p => p.global_code == "S104"),
                    clean_up = output.FirstOrDefault(p => p.global_code == "S105"),
                    last_check = output.FirstOrDefault(p => p.global_code == "S106"),
                    safety_meeting = output.FirstOrDefault(p => p.global_code == "S107"),
                    spq_sign = [.. lstSign.Where(p => p.sign_type == (int)SignTypeEnum.scr_spq)],
                    wpic_sign = [.. lstSign.Where(p => p.sign_type == (int)SignTypeEnum.scr_wpic)],
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 合格人员职务检查表 CPD （CPDAS） 
        /// 单独接口：两层，第一层为列头；第二层为数据（单选）
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="lstItems"></param>
        /// <param name="lstItemValues"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCpdAsync(
            Guid recordId, 
            List<Sys_Site_Work_Check_Item> lstItems,
            List<Biz_Site_Work_Record_Item_Check> lstItemValues)
        {
            try
            {
                var cpdPrefixes = new[] { "C101", "C102", "C103" };
                bool HasPrefix(string code) => !string.IsNullOrEmpty(code) && cpdPrefixes.Any(p => code.StartsWith(p));

                var vals = lstItemValues;
                var l1s = lstItems.Where(i => i.level == 1 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();
                var l2s = lstItems.Where(i => i.level == 2 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();

                var output = new List<OptionTableDto>();
                foreach (var h in l1s)
                {
                    var tbl = new OptionTableDto
                    {
                        header_code = h.item_code,
                        header_cht = h.name_cht,
                        header_eng = h.name_eng,
                        global_code = h.global_code
                    };
                    var rows = l2s.Where(x => x.master_id == h.item_code).ToList();
                    foreach (var r in rows)
                    {
                        var val = vals.FirstOrDefault(v => v.check_code == r.item_code);
                        tbl.rows.Add(new OptionRowDto
                        {
                            code = r.item_code,
                            name_cht = r.name_cht,
                            name_eng = r.name_eng,
                            global_code = r.global_code,
                            value = MakeValue(val)
                        });
                    }
                    output.Add(tbl);
                }

                // 获取签名
                var lstSign = await GetSignAsync(recordId, [(int)SignTypeEnum.cpd_spq, (int)SignTypeEnum.cpd_check]);

                var result = new CPDASDto
                {
                    before_work = output.FirstOrDefault(p => p.global_code == "C101"),
                    working = output.FirstOrDefault(p => p.global_code == "C102"),
                    after_work = output.FirstOrDefault(p => p.global_code == "C103"),
                    //cpd_data = output,
                    spq_sign = [.. lstSign.Where(p => p.sign_type == (int)SignTypeEnum.cpd_spq)],
                    check_sign = [.. lstSign.Where(p => p.sign_type == (int)SignTypeEnum.cpd_check)],
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 质量检查表 QDC （Quality Checklist） 
        /// 单独接口：两层，第一层为列头；第二层为数据（单选）
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="lstItems"></param>
        /// <param name="lstItemValues"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQdcAsync(
            Guid recordId, 
            List<Sys_Site_Work_Check_Item> lstItems,
            List<Biz_Site_Work_Record_Item_Check> lstItemValues)
        {
            try
            {
                var qdcPrefixes = new[] { "C201", "C202", "C203", "C204", "C205" };
                bool HasPrefix(string code) => !string.IsNullOrEmpty(code) && qdcPrefixes.Any(p => code.StartsWith(p));


                var vals = lstItemValues;
                var l1s = lstItems.Where(i => i.level == 1 && HasPrefix(i.global_code)).OrderBy(i => i.order_no).ToList();
                var l2s = lstItems.Where(i => i.level == 2 && HasPrefix(i.global_code)).ToList();

                var output = new List<OptionTableDto>();
                foreach (var h in l1s)
                {
                    var tbl = new OptionTableDto
                    {
                        header_code = h.item_code,
                        header_cht = h.name_cht,
                        header_eng = h.name_eng,
                        global_code = h.global_code
                    };
                    var rows = l2s.Where(x => x.master_id == h.item_code).OrderBy(p => p.order_no).ToList();
                    foreach (var r in rows)
                    {
                        var val = vals.FirstOrDefault(v => v.check_code == r.item_code);
                        tbl.rows.Add(new OptionRowDto
                        {
                            code = r.item_code,
                            name_cht = r.name_cht,
                            name_eng = r.name_eng,
                            global_code = r.global_code,
                            value = MakeValue(val)
                        });
                    }
                    output.Add(tbl);
                }

                // 获取签名
                var lstSign = await GetSignAsync(recordId, [(int)SignTypeEnum.qdc_wpic]);

                var result = new QualityCheckDto
                {
                    qualifications_training = output.FirstOrDefault(p => p.global_code == "C201"),
                    site_work_file = output.FirstOrDefault(p => p.global_code == "C202"),
                    quality_management = output.FirstOrDefault(p => p.global_code == "C203"),
                    process_quality_control = output.FirstOrDefault(p => p.global_code == "C204"),
                    others = output.FirstOrDefault(p => p.global_code == "C205"),
                    //qdc_data = output,
                    check_sign = lstSign,
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// SIC合格人員(轨道) CP （SIC CP(T)） 
        /// 单独接口：三层聚合，第二层行；第三层名称聚合
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="lstItems"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCpAsync(
            Guid recordId, 
            List<Sys_Site_Work_Check_Item> lstItems, 
            List<Biz_Site_Work_Record_Item_Check> lstItemValues)
        {
            try
            {
                var valid = (int)Core.Enums.SystemDataStatus.Valid;
                var cpPrefixes = new[] { "SI101", "SI102", "SI103", "SI104" };
                bool HasPrefix(string code) => !string.IsNullOrEmpty(code) && cpPrefixes.Any(p => code.StartsWith(p));


                var l1s = lstItems.Where(i => i.level == 1 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();
                var l2s = lstItems.Where(i => i.level == 2 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();
                var l3s = lstItems.Where(i => i.level == 3 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();
                var vals = lstItemValues;

                var output = new List<GroupedSectionDto>();
                foreach (var t in l1s)
                {
                    var section = new GroupedSectionDto
                    {
                        title_code = t.item_code,
                        title_cht = t.name_cht,
                        title_eng = t.name_eng,
                        global_code = t.global_code
                    };
                    var rows2 = l2s.Where(x => x.master_id == t.item_code).ToList();
                    foreach (var r2 in rows2)
                    {
                        var children3 = l3s.Where(x => x.master_id == r2.item_code).ToList();
                        var namesCht = string.Join("\n", children3.Select(x => x.name_cht));
                        var namesEng = string.Join("\n", children3.Select(x => x.name_eng));
                        var val = vals.FirstOrDefault(v => v.check_code == r2.item_code);

                        section.rows.Add(new GroupedRowDto
                        {
                            level2_code = r2.item_code,
                            level2_name_cht = r2.name_cht,
                            level2_name_eng = r2.name_eng,
                            level3_names_cht = namesCht,
                            level3_names_eng = namesEng,
                            global_code = r2.global_code,
                            value = MakeValue(val)
                        });
                    }
                    output.Add(section);
                }

                var otherData = vals.FirstOrDefault(p => p.check_code == -1 || p.check_code == null);
                var remarkData = otherData?.text_result;

                // 获取签名
                var lstSign = await GetSignAsync(recordId, [(int)SignTypeEnum.cp_siccp]);

                var result = new SICCPDto
                {
                    sic_data = await GetSICCheckData(recordId),
                    prior_to_track_access = output.FirstOrDefault(p => p.global_code == "SI101"),
                    after_authorized = output.FirstOrDefault(p => p.global_code == "SI102"),
                    before_track_clear = output.FirstOrDefault(p => p.global_code == "SI103"),
                    after_track_clear = output.FirstOrDefault(p => p.global_code == "SI104"),
                    remark = remarkData,
                    //cp_data = output,
                    sign = lstSign,
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// SIC复检 SIC 
        /// 单独接口：三层聚合，第二层行；第三层名称聚合
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="lstItems"></param>
        /// <param name="lstItemValues"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSicAsync(
            Guid recordId, 
            List<Sys_Site_Work_Check_Item> lstItems, 
            List<Biz_Site_Work_Record_Item_Check> lstItemValues)
        {
            try
            {
                var valid = (int)Core.Enums.SystemDataStatus.Valid;
                var sicPrefixes = new[] { "SI201", "SI202", "SI203", "SI204" };
                bool HasPrefix(string code) => !string.IsNullOrEmpty(code) && sicPrefixes.Any(p => code.StartsWith(p));

                var l1s = lstItems.Where(i => i.level == 1 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();
                var l2s = lstItems.Where(i => i.level == 2 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();
                var l3s = lstItems.Where(i => i.level == 3 && HasPrefix(i.global_code)).OrderBy(i => i.global_code).ToList();
                var vals = lstItemValues;

                var output = new List<GroupedSectionDto>();
                foreach (var t in l1s)
                {
                    var section = new GroupedSectionDto
                    {
                        title_code = t.item_code,
                        title_cht = t.name_cht,
                        title_eng = t.name_eng,
                        global_code = t.global_code
                    };
                    var rows2 = l2s.Where(x => x.master_id == t.item_code).ToList();
                    foreach (var r2 in rows2)
                    {
                        var children3 = l3s.Where(x => x.master_id == r2.item_code).ToList();
                        var namesCht = string.Join("\n", children3.Select(x => x.name_cht));
                        var namesEng = string.Join("\n", children3.Select(x => x.name_eng));
                        var val = vals.FirstOrDefault(v => v.check_code == r2.item_code);
                        section.rows.Add(new GroupedRowDto
                        {
                            level2_code = r2.item_code,
                            level2_name_cht = r2.name_cht,
                            level2_name_eng = r2.name_eng,
                            level3_names_cht = namesCht,
                            level3_names_eng = namesEng,
                            global_code = r2.global_code,
                            value = MakeValue(val)
                        });
                    }
                    output.Add(section);
                }

                var otherData = vals.FirstOrDefault(p => p.check_code == -1 || p.check_code == null);
                var remarkData = otherData?.remark;


                // 获取签名
                var lstSign = await GetSignAsync(recordId, [(int)SignTypeEnum.sic_sic]);

                var result = new SICCPDto
                {
                    sic_data = await GetSICCheckData(recordId),
                    prior_to_track_access = output.FirstOrDefault(p => p.global_code == "SI201"),
                    after_authorized = output.FirstOrDefault(p => p.global_code == "SI202"),
                    before_track_clear = output.FirstOrDefault(p => p.global_code == "SI203"),
                    after_track_clear = output.FirstOrDefault(p => p.global_code == "SI204"),
                    remark = remarkData,
                    //sic_data = output,
                    sign = lstSign,
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 设置SIC清单（选择SIC人员和参考编号）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SetSICDataAsync(EditCheckSICdAT input)
        {
            try
            {
                var recordData = await _siteWorkRecordRepository
                  .FindAsIQueryable(p => p.id == input.record_id)
                  .FirstOrDefaultAsync();
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                var workerData = await _workerRepository.FindAsyncFirst(p => p.id == input.sic_worker_id);
                if (workerData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }

                recordData.check_ref = input.sic_ref;
                recordData.modify_id = UserContext.Current.UserId;
                recordData.modify_date = DateTime.Now;
                recordData.modify_name = UserContext.Current.UserName;

                _siteWorkRecordRepository.Update(recordData);

                workerData.is_sic = 1;
                workerData.modify_id = UserContext.Current.UserId;
                workerData.modify_date = DateTime.Now;
                workerData.modify_name = UserContext.Current.UserName;
                _workerRepository.Update(workerData);

                await _repository.SaveChangesAsync();


                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        private async Task<List<SiteWorkCheckSignDto>> GetSignAsync(Guid recordId, List<int?> lstSignType)
        {
            var valid = (int)SystemDataStatus.Valid;

            // 获取工人列表
            var lstWorker = await _workerRepository
                .FindAsIQueryable(p => p.record_id == recordId && p.delete_status == valid)
                .ToListAsync();

            // 获取联系人列表
            var contactIds = lstWorker.Select(s => s.contact_id).Distinct().ToList();
            var contacts = await _contactRepository
               .FindAsIQueryable(ct => ct.delete_status == valid && contactIds.Contains(ct.id))
               .AsNoTracking()
               .ToListAsync();
            var dicContact = contacts.ToDictionary(c => c.id, c => c);

            // 查询签名记录：按 record_id 与 relation_type
            var signs = await _signRepository
                .FindAsIQueryable(s => s.record_id == recordId && s.delete_status == valid)
                .AsNoTracking()
                .ToListAsync();
          
            var lstData = new List<SiteWorkCheckSignDto>();
            foreach (var signType in lstSignType)
            {
                if (!signType.HasValue)
                {
                    continue;
                }
                var lstSign = signs.Where(p => p.relation_type == signType).ToList();
                if (signType == (int)SignTypeEnum.worker)
                {
                    continue;
                }

                var jobVale = "";
                var lstSignWorker = new List<Biz_Site_Work_Record_Worker>();
                switch (signType)
                {
                    case (int)SignTypeEnum.scr_spq:
                    case (int)SignTypeEnum.cpd_spq:
                    case (int)SignTypeEnum.cp_siccp:
                        jobVale = "CP(T)";
                        lstSignWorker = [.. lstWorker.Where(p => p.is_cp == 1)];
                        break;
                    case (int)SignTypeEnum.scr_wpic:
                    case (int)SignTypeEnum.qdc_wpic:
                        jobVale = "WPIC";
                        lstSignWorker = [.. lstWorker.Where(p => p.is_wpic == 1)];
                        break;
                    case (int)SignTypeEnum.cpd_check:
                        if (lstSign.Count > 0)
                        {
                            foreach (var item in lstSign)
                            {
                                lstData.Add(new SiteWorkCheckSignDto
                                {
                                    id = item.id,
                                    sign_type = (int)SignTypeEnum.cpd_check,
                                    sign_type_name = SignTypeHelper.GetShiftTypeStr((int)SignTypeEnum.cpd_check),
                                    sign_image = GetSignImage(item)
                                });
                            }
                        }
                        else
                        {
                            lstData.Add(new SiteWorkCheckSignDto
                            {
                                sign_type = (int)SignTypeEnum.cpd_check,
                                sign_type_name = SignTypeHelper.GetShiftTypeStr((int)SignTypeEnum.cpd_check),
                                sign_image = null
                            });
                        }
                            
                        break;
                    case (int)SignTypeEnum.sic_sic:
                        lstSignWorker = [.. lstWorker.Where(p => p.is_sic == 1)];
                        break;
                    default:
                        break;
                }
                if (signType == (int)SignTypeEnum.cpd_check)
                {
                    continue;
                }

                lstData.AddRange(SetSignData(lstSignWorker, signs, dicContact, signType.Value, jobVale));
            }
            return lstData;
        }

        /// <summary>
        /// 设置签名数据
        /// </summary>
        /// <param name="lstWorkers"></param>
        /// <param name="lstSigns"></param>
        /// <param name="dicContacts"></param>
        /// <param name="signType"></param>
        /// <returns></returns>
        private List<SiteWorkCheckSignDto> SetSignData(
            List<Biz_Site_Work_Record_Worker> lstWorkers,
            List<Biz_Site_Work_Record_Sign> lstSigns,
            Dictionary<Guid, Sys_Contact> dicContacts,
            int signType,
            string jobValue)
        {
            var result = new List<SiteWorkCheckSignDto>();

            foreach (var item in lstWorkers)
            {
                var data = new SiteWorkCheckSignDto
                {
                    sign_id = item.id,
                    sign_type = signType,
                    sign_type_name = SignTypeHelper.GetShiftTypeStr(signType),
                    sign_job = jobValue
                };

                if (item.contact_id.HasValue) 
                {
                    var isGetContactOk = dicContacts.TryGetValue(item.contact_id.Value, out var contactData);
                    if (isGetContactOk)
                    {
                        data.sign_name_cht = contactData.name_cht;
                        data.sign_name_eng = contactData.name_eng;
                    }
                }


                var signData = signType == (int)SignTypeEnum.cpd_check ?
                                           lstSigns.FirstOrDefault(p => p.relation_type == signType) :
                                           lstSigns.FirstOrDefault(p => p.relation_id == item.id && p.relation_type == signType);
                if (signData != null)
                {
                    data.id = signData.id;
                    data.sign_image = GetSignImage(signData);
                }

                result.Add(data);
            }

            return result;
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="signData"></param>
        /// <returns></returns>
        private byte[] GetSignImage(Biz_Site_Work_Record_Sign signData)
        {
            if (signData == null)
            {
                return null;
            }
            byte[] imageBytes = null;
            try
            {
                var root = AppSetting.FileSaveSettings.FolderPath;
                var relative = signData.file_blurry_path ?? string.Empty;
                relative = relative.TrimStart('\\', '/'); // 清理前导分隔符，避免 Path.Combine 忽略根目录
                var fullPath = Path.Combine(root, relative);
                if (File.Exists(fullPath))
                {
                    imageBytes = File.ReadAllBytes(fullPath);
                }
            }
            catch (Exception)
            {

            }
            return imageBytes;
        }

        /// <summary>
        /// 统一赋值：radio为空=>-1；text为空=>""；选中为空=>false，时间为空=> null
        /// </summary>
        /// <param name="radio"></param>
        /// <param name="check"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private SiteWorkCheckValueDto MakeValue(Biz_Site_Work_Record_Item_Check checkValue)
        {
            var radio = checkValue?.radio_result;
            var check = checkValue?.check_result;
            var text = checkValue?.text_result;
            var time = checkValue?.time_result;

            return new SiteWorkCheckValueDto
            {
                radio_result = radio.HasValue ? radio.Value : -1,
                is_checked = check.HasValue && check.Value == 1,
                text_value = text ?? string.Empty,
                time_result = time,
                remark = checkValue?.remark
            };
        }

        /// <summary>
        /// 获取轨道清单内容
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        private async Task<CheckSicDataDto> GetSICCheckData(Guid recordId)
        {
            // 获取工地记录
            var recordData = await _siteWorkRecordRepository
                .FindAsIQueryable(p => p.id == recordId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (recordData == null)
            {
                return null;
            }
            var siteData = await _siteRepository
                .FindAsIQueryable(p => p.id == recordData.site_id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var lstWorks = await _workerRepository
                 .FindAsIQueryable(p => p.record_id == recordId && (p.is_cp == 1 || p.is_sic == 1))
                 .AsNoTracking()
                 .ToListAsync();
            var workCP = lstWorks.FirstOrDefault(p => p.is_cp == 1);
            var contectIdCP = workCP == null ? Guid.Empty : workCP.contact_id.Value;
            var workSIC = lstWorks.FirstOrDefault(p => p.is_sic == 1);
            var contectIdSIC = workSIC == null ? Guid.Empty : workSIC.contact_id.Value;

            var lstContect = await _contactRepository
                .FindAsIQueryable(p => p.id == contectIdCP || p.id == contectIdSIC)
                .AsNoTracking()
                .ToListAsync();
            var contectCP = lstContect.FirstOrDefault(p => p.id == contectIdCP);
            var contectSIC = lstContect.FirstOrDefault(p => p.id == contectIdSIC);

            return new CheckSicDataDto
            {
                record_id = recordData.id,
                sic_ref = recordData.check_ref,
                site_id = recordData.site_id,
                site_sho = siteData.name_sho,
                cp_worker_id = workCP?.id,
                cp_worker_cht = contectCP?.name_cht,
                cp_worker_eng = contectCP?.name_eng,
                sic_worker_id = workSIC?.id,
                sic_worker_cht = contectSIC?.name_cht,
                sic_worker_eng = contectSIC?.name_eng,
            };
        }

        /// <summary>
        /// 获取选择内容
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        private async Task<List<Sys_Site_Work_Check_Item>> GetCheckItemByRecordIdAsync(Guid recordId, int typeCode)
        {
            var allItems = await _sysCheckItemRepository
                 .FindAsIQueryable(i => i.enable == 1)
                 .AsNoTracking()
                 .ToListAsync();

            var recordData = await _siteWorkRecordRepository.FindAsyncFirst(p => p.id == recordId);
            if (recordData != null && !string.IsNullOrEmpty(recordData.check_config))
            {
                try
                {
                    var setting = JsonConvert.DeserializeObject<CheckCodeSetting>(recordData.check_config);
                    switch (typeCode)
                    {
                        case 0:
                            return [.. allItems.Where(p => setting.brf_setting.Contains(p.item_code))];
                        case 1:
                            return [.. allItems.Where(p => setting.scr_setting.Contains(p.item_code))];
                        case 2:
                            return [.. allItems.Where(p => setting.cpd_setting.Contains(p.item_code))];
                        case 3:
                            return [.. allItems.Where(p => setting.qdc_setting.Contains(p.item_code))];
                        case 4:
                            return [.. allItems.Where(p => setting.cp_setting.Contains(p.item_code))];
                        case 5:
                            return [.. allItems.Where(p => setting.sic_setting.Contains(p.item_code))];
                        default:
                            return [.. allItems.Where(p => setting.brf_setting.Contains(p.item_code) ||
                                                           setting.scr_setting.Contains(p.item_code) ||
                                                           setting.qdc_setting.Contains(p.item_code) ||
                                                           setting.cp_setting.Contains(p.item_code)  ||
                                                           setting.sic_setting.Contains(p.item_code))
                                   ];
                    }

                    
                }
                catch (Exception)
                {
                }
            }


            return allItems;
        }

        #endregion

        #region 设置值

        /// <summary>
        /// 设置选项值内容（单条，实时）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SetCheckItemBySingleAsync(SetItemValueDataDto input)
        {
            try
            {
                var itemValue = await _repository
                   .FindAsIQueryable(p => p.record_id == input.record_id &&
                                          p.check_type == input.item_type &&
                                          p.check_code == input.item_code &&
                                          p.delete_status == (int)SystemDataStatus.Valid)
                   .FirstOrDefaultAsync();

                if (itemValue != null) 
                {
                    itemValue.text_result = input.text_value;
                    itemValue.radio_result = input.radio_result ?? -1;
                    itemValue.check_result = input.is_checked ? 1 : 0;
                    itemValue.time_result = input.time_result;
                    itemValue.remark = input.remark;
                    itemValue.modify_id = UserContext.Current.UserId;
                    itemValue.modify_date = DateTime.Now;
                    itemValue.modify_name = UserContext.Current.UserName;

                    _repository.Update(itemValue);
                }
                else
                {
                    itemValue = new Biz_Site_Work_Record_Item_Check
                    {
                        id = Guid.NewGuid(),
                        create_id = UserContext.Current.UserId,
                        create_date = DateTime.Now,
                        create_name = UserContext.Current.UserName,
                        delete_status = (int)SystemDataStatus.Valid,

                        record_id = input.record_id,
                        check_code = input.item_code,
                        text_result = input.text_value,
                        radio_result = input.radio_result ?? -1,
                        check_result = input.is_checked ? 1 : 0,
                        check_type = input.item_type,
                        time_result = input.time_result,
                        remark = input.remark
                    };

                    _repository.Add(itemValue);
                }

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 保存签名文件
        /// </summary>
        /// <param name="signInput"></param>
        /// <param name="signPic"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SetItemSignBySingleAsync(SetItemSignDto signInput, IFormFile signPic)
        {
            try
            {
                var isPic = _projectfilesService.CheckFileIsPhoto(new List<IFormFile> { signPic });
                if (!isPic)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("site_work_photo_type_error"));
                }

                // 获取工地记录
                var recordData = await _siteWorkRecordRepository
                    .FindAsIQueryable(p => p.id == signInput.record_id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                if (recordData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                var siteData = await _siteRepository.FindFirstAsync(p => p.id == recordData.site_id);

                // 如果是工人签名的话，需要更改工地工作记录的是否签名
                if (signInput.sign_type == 0)
                {
                    var workerData = await _workerRepository.FindAsyncFirst(p => p.id == signInput.sign_id);
                    if (workerData != null)
                    {
                        workerData.is_sign = 1;
                        _workerRepository.Update(workerData);
                    }
                }

                // 获取放置文件夹
                var getFolderResult = await _projectfilesService
                    .GetFileFolderByContactIdAsync(recordData.contract_id.Value, UploadFileCode.Site_Work_Record);
                if (!getFolderResult.status)
                {
                    return getFolderResult;
                }
                var strRelFolder = getFolderResult.data as string; // 相对路径
                if (siteData != null && !string.IsNullOrEmpty(siteData.name_sho))
                {
                    strRelFolder = Path.Combine(strRelFolder, siteData.name_sho);
                }
                if (recordData.work_date.HasValue)
                {
                    strRelFolder = Path.Combine(strRelFolder, recordData.work_date?.ToString("yyyy-MM-dd") + "\\sign");
                }
                var strAbsFolder = Path.Combine(AppSetting.FileSaveSettings.FolderPath, strRelFolder); // 绝对路径
                if (!Directory.Exists(strAbsFolder))
                {
                    Directory.CreateDirectory(strAbsFolder);
                }

                // 处理文件
                var strExt = Path.GetExtension(signPic.FileName).TrimStart('.').ToLower(); // 扩展名

                // 模糊文件
                var strBlurrFileName = $"sign_blurry_image.{strExt}";                      // 模糊化文件名
                var strBlurrFileRelPath = Path.Combine(strRelFolder, strBlurrFileName);    // 模糊化文件-相对路径
                var strBlurrFileAbsPath = Path.Combine(strAbsFolder, strBlurrFileName);    // 模糊化文件-绝对路径
                if (File.Exists(strBlurrFileAbsPath))
                {
                    // 如果文件存在则添加(+1)
                    strBlurrFileName = FileHelper.EnsureUniqueFileName(strAbsFolder, strBlurrFileName);
                    strBlurrFileRelPath = Path.Combine(strRelFolder, strBlurrFileName);    // 保存数据库中的相对路径
                    strBlurrFileAbsPath = Path.Combine(strAbsFolder, strBlurrFileName);    // 文件绝对路径
                }

                // 加密文件
                var strFileName = $"sign_image.enc";                                       // 加密文件名
                //var strFileName1 = $"sign_image3333.png";                                  // 加密文件名
                var strFileRelPath = Path.Combine(strRelFolder, strFileName);              // 加密文件-相对路径
                var strFileAbsPath = Path.Combine(strAbsFolder, strFileName);              // 加密文件-绝对路径
                //var strFile123AbsPath = Path.Combine(strAbsFolder, strFileName1);              // 加密文件-绝对路径
                if (File.Exists(strFileAbsPath))
                {
                    // 如果文件存在则添加(+1)
                    strFileName = FileHelper.EnsureUniqueFileName(strAbsFolder, strFileName);
                    strFileRelPath = Path.Combine(strRelFolder, strFileName);              // 保存数据库中的相对路径
                    strFileAbsPath = Path.Combine(strAbsFolder, strFileName);              // 文件绝对路径
                }

                // 保存文件
                using (var fileSteam = signPic.OpenReadStream())
                {
                    ImageHelper.MosaicImageToFile(fileSteam, 10, strBlurrFileAbsPath);
                }
                using (var fileSteam = signPic.OpenReadStream())
                {
                    ImageHelper.EncryptImageStreamToFile(
                        fileSteam,
                        "adfcvON12*DA/45*-FGA/*AWER/++ASDF+/v*fg./hsxxdh123nl/a*fz-a", 
                        strFileAbsPath);
                }
                //using (var test = new FileStream(strFileAbsPath, FileMode.Open))
                //{
                //    ImageHelper.DecryptImageStreamToFile(
                //        test,
                //        "adfcvON12*DA/45*-FGA/*AWER/++ASDF+/v*fg./hsxxdh123nl/a*fz-a",
                //        strFile123AbsPath);
                //}

                var signData = await _signRepository
                    .FindAsIQueryable(p => p.record_id == signInput.record_id &&
                                           p.relation_id == signInput.sign_id &&
                                           p.relation_type == signInput.sign_type &&
                                           p.delete_status == (int)SystemDataStatus.Valid)
                    .FirstOrDefaultAsync();

                if (signData != null)
                {
                    if (!string.IsNullOrEmpty(signData.file_path))
                    {
                        _sysFileRecordsService.MoveFileToTemporary(signData.file_path, Path.GetFileName(signData.file_path), Path.GetExtension(signData.file_path));
                    }
                    if (!string.IsNullOrEmpty(signData.file_blurry_path))
                    {
                        _sysFileRecordsService.MoveFileToTemporary(signData.file_blurry_path, Path.GetFileName(signData.file_blurry_path), Path.GetExtension(signData.file_blurry_path));
                    }
                    
                    signData.modify_id = UserContext.Current.UserId;
                    signData.modify_date = DateTime.Now;
                    signData.modify_name = UserContext.Current.UserName;

                    signData.file_path = strFileRelPath;
                    signData.file_blurry_path = strBlurrFileRelPath;

                    _signRepository.Update(signData);
                }
                else
                {
                    signData = new Biz_Site_Work_Record_Sign
                    {
                        id = Guid.NewGuid(),
                        create_id = UserContext.Current.UserId,
                        create_date = DateTime.Now,
                        create_name = UserContext.Current.UserName,
                        delete_status = (int)SystemDataStatus.Valid,

                        record_id = signInput.record_id,
                        relation_type = signInput.sign_type,
                        file_path = strFileRelPath,
                        file_blurry_path = strBlurrFileRelPath,
                        relation_id = signInput.sign_id,

                    };
                    _signRepository.Add(signData);
                }

                await _signRepository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 设置选项值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SetCheckItemValueAsync(SetIemValueDto input)
        {
            try
            {
                var lstCodes = input.value_items.Select(p => p.item_code).ToList();

                // 这里是要修改的
                var lstOldValues = await _repository
                    .FindAsIQueryable(p => p.record_id == input.record_id &&
                                           p.check_type == input.check_type &&
                                           p.delete_status == (int)SystemDataStatus.Valid &&
                                           lstCodes.Contains(p.check_type))
                    .ToDictionaryAsync(p => p.check_code);

                var lstNewValues = new List<Biz_Site_Work_Record_Item_Check>();
                var lstEditValues = new List<Biz_Site_Work_Record_Item_Check>();

                foreach (var item in input.value_items)
                {
                    var isExist = lstOldValues.TryGetValue(item.item_code, out Biz_Site_Work_Record_Item_Check checkItem);
                    if (isExist) 
                    {
                        checkItem.text_result = item.text_value;
                        checkItem.radio_result = item.radio_result ?? -1;
                        checkItem.check_result = item.is_checked ? 1 : 0;
                        checkItem.modify_id = UserContext.Current.UserId;
                        checkItem.modify_date = DateTime.Now;
                        checkItem.modify_name = UserContext.Current.UserName;

                        lstEditValues.Add(checkItem);
                        continue;
                    }

                    lstNewValues.Add(new Biz_Site_Work_Record_Item_Check
                    {
                        id = Guid.NewGuid(),
                        create_id = UserContext.Current.UserId,
                        create_date = DateTime.Now,
                        create_name = UserContext.Current.UserName,
                        delete_status = (int)SystemDataStatus.Valid,

                        record_id = input.record_id,
                        check_code = item.item_code,
                        text_result = item.text_value,
                        radio_result = item.radio_result ?? -1,
                        check_result = item.is_checked ? 1 : 0
                    });
                }

                _repository.AddRange(lstNewValues);
                _repository.UpdateRange(lstEditValues);

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion


        
    }
}

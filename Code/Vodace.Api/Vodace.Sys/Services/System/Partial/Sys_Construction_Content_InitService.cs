
using AutoMapper;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
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
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Sys_Construction_Content_InitService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Construction_Content_InitRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ISys_File_RecordsRepository _repositoryFileRecord;//访问数据库
        private readonly ISys_File_RecordsService _FileRecordsService;

        [ActivatorUtilitiesConstructor]
        public Sys_Construction_Content_InitService(
            ISys_Construction_Content_InitRepository dbRepository,
            ISys_File_RecordsRepository repositoryFileRecord,
            IConfiguration configuration,
            IMapper mapper,
            ILocalizationService localizationService,
            ISys_File_RecordsService file_RecordsService,
            IHttpContextAccessor httpContextAccessor,
            ISys_File_RecordsService fileRecordsService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            _configuration = configuration;
            _repositoryFileRecord = repositoryFileRecord;
            _FileRecordsService = fileRecordsService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public WebResponseContent Add(ConstructionContentInitDto dto)
        {
            try
            {
                Sys_Construction_Content_Init model = new Sys_Construction_Content_Init();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                if (string.IsNullOrEmpty(dto.work_type))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "work_type"));

                if (string.IsNullOrEmpty(dto.item_code))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "item_code"));

                if (string.IsNullOrEmpty(dto.content))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "content"));

                var exist = _repository.Find(a => a.work_type == dto.work_type && a.item_code == dto.item_code && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                if (exist != null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_exist", dto.item_code));
                }

                model.line_number = GetLastLinNumber(dto.work_type, dto.master_id) + 1;
                model.level = dto.item_code.Count(a => a == '.') + 1;
                model.master_id = dto.master_id;
                model.external_link_id = dto.external_link_id;
                model.item_code = dto.item_code;
                model.content = dto.content;
                model.work_type = dto.work_type;
                model.point_type = dto.point_type;
                model.extend_attr = dto.extend_attr;

                _repository.Add(model, true);
                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Construction_Content_InitService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                var model = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (model != null)
                {
                    //是否存在子级数据
                    var hasChild = _repository.Find(a => a.master_id == guid && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                    if(hasChild.Count > 0)
                        return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("sub_level_not_allowed")}");

                    model.delete_status = (int)SystemDataStatus.Invalid;
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;
                    var res = _repository.Update(model, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("delete") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Construction_Content_InitService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(ConstructionContentInitEditDto dto)
        {
            try
            {
                var model = repository.Find(p => p.id == dto.id).FirstOrDefault();
                if (model != null)
                {
                    model.delete_status = (int)SystemDataStatus.Valid;
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    if (string.IsNullOrEmpty(dto.work_type))
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "work_type"));

                    if (string.IsNullOrEmpty(dto.item_code))
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "item_code"));

                    if (string.IsNullOrEmpty(dto.content))
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "content"));

                    var exist = _repository.Find(a => a.work_type == dto.work_type && a.item_code == dto.item_code && a.id != dto.id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                    if (exist != null)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_exist", dto.item_code));
                    }

                    model.line_number = GetLastLinNumber(dto.work_type, dto.master_id);
                    model.level = dto.item_code.Count(a => a == '.') + 1;
                    model.master_id = dto.master_id;
                    model.external_link_id = dto.external_link_id;
                    model.item_code = dto.item_code;
                    model.content = dto.content;
                    model.work_type = dto.work_type;
                    model.point_type = dto.point_type;
                    model.extend_attr = dto.extend_attr;

                    var res = _repository.Update(model, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{dto.id}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Construction_Content_InitService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetContentInitList(PageInput<ConstructionContentInitEditDto> dto)
        {
            try
            {
                // 构建查询，实现三表左连接
                var query = from r in _repository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid)
                            select new ConstructionContentInitListDto()
                            {
                                // 获取请假记录所有字段
                                id = r.id,

                                line_number = r.line_number,
                                level = r.level,
                                master_id = r.master_id,
                                external_link_id = r.external_link_id,
                                item_code  =r.item_code,
                                content = r.content,
                                work_type = r.work_type,
                                point_type = r.point_type,
                                extend_attr = r.extend_attr,

                                create_id = r.create_id,
                                create_name = r.create_name,
                                create_date = r.create_date,
                                modify_id = r.modify_id,
                                modify_name = r.modify_name,
                                modify_date = r.modify_date,
                            };

                // 获取查询条件
                var searchDto = dto.search;
                // 添加查询条件过滤
                if (searchDto != null)
                {
                    query = query.WhereIF(searchDto.line_number.HasValue, p => p.line_number == searchDto.line_number);
                    query = query.WhereIF(searchDto.level.HasValue, p => p.level == searchDto.level);                    

                    if(searchDto.master_id == Guid.Empty)
                    {
                        query = query.WhereIF(true, p => p.master_id == null);
                    }
                    else
                    {
                        query = query.WhereIF(searchDto.master_id.HasValue, p => p.master_id == searchDto.master_id);
                    }

                    query = query.WhereIF(searchDto.id.HasValue, p => p.id == searchDto.id);
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.item_code), p => p.item_code.Contains(searchDto.item_code));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.content), p => p.content.Contains(searchDto.content));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.work_type), p => p.work_type.Contains(searchDto.work_type));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.point_type), p => p.point_type.Contains(searchDto.point_type));
                }
                
                if (!string.IsNullOrEmpty(dto.sort_field))
                    query = query.OrderByDynamic(dto.sort_field, dto.sort_type);
                else
                {
                    //query = query.OrderBy(x => x.line_number);
                    Dictionary<string, QueryOrderBy> orderByDict = new Dictionary<string, QueryOrderBy>
                    {
                        { "work_type", QueryOrderBy.Asc },
                        { "line_number", QueryOrderBy.Asc },
                        { "item_code", QueryOrderBy.Asc }
                    };
                    query = query.GetIQueryableOrderBy(orderByDict);
                }

                var sql = query.ToQueryString();

                // 执行分页查询（使用项目提供的扩展方法）
                var result = await query.GetPageResultAsync(dto);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Construction_Content_InitService.GetContentInitList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetContentIntiWorkTypeList()
        {
            try
            {
                var result = await _repository.FindAsIQueryable(a => a.delete_status == (int)SystemDataStatus.Valid).Select(a => a.work_type).Distinct().ToListAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Construction_Content_InitService.GetContentInitList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 导入工程任务
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent ImportData(IFormFile file)
        {
            try
            {
                #region  -- 文件导入 --

                //读取文件配置地址
                var strFolderPath = _FileRecordsService.GetFileSaveFolder(UploadFileCode.Construction_Content_Init);

                //保存文件
                var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strFolderPath.data as string);
                var fileInfo = saveFileResult.data as List<FileInfoDto>;
                if (fileInfo == null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                }

                //保存数据库
                var fileData = new Sys_File_Records
                {
                    id = Guid.NewGuid(),
                    master_id = null,
                    remark = "",
                    file_name = fileInfo[0].file_name,
                    file_ext = fileInfo[0].file_ext,
                    file_path = fileInfo[0].file_relative_path,
                    file_code = UploadFileCode.Quotation_Record_Documents,
                    file_size = (int)file.Length,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now,
                    upload_status = (int)UploadStatus.Finish
                };
                _repositoryFileRecord.AddAsync(fileData);

                #endregion

                #region  -- 读取excel文档 --
                
                string filePath = Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileInfo[0].file_relative_path);
                
                DateTime nowTime = DateTime.Now;
                string strEmpty = string.Empty;
                string item_code = string.Empty;
                string content = string.Empty;
                string item_code_child = string.Empty;
                string content_child = string.Empty;
                string quantity = string.Empty;
                string master_id = string.Empty;
                int _level = 1;

                bool isExist = false;
                List<Sys_Construction_Content_Init> lstCCI = new List<Sys_Construction_Content_Init>();
                List<Sys_Construction_Content_Init> lstCCI_child = new List<Sys_Construction_Content_Init>();

                //读取Pre Work
                int line_number = 0;
                var sheetIndex = _configuration.GetSection("engineering_details:pre_work:sheet_index").Value;
                var rowStart = _configuration.GetSection("engineering_details:pre_work:row_start").Value;
                var colStart = _configuration.GetSection("engineering_details:pre_work:col_start").Value;
                (var pre_work, var pre_work_child) = NPOIHelper.ReadPreWorkAsKeyValuePairs(filePath, int.Parse(sheetIndex.ToString()), int.Parse(rowStart.ToString()), int.Parse(colStart.ToString()));                
                nowTime = DateTime.Now;
                if (pre_work != null && pre_work_child != null && pre_work_child.Count > 0 && pre_work.Count > 0)
                {
                    for (var i = 0; i < pre_work.Count; i++)
                    {
                        line_number++;
                        item_code = string.Empty;
                        content = string.Empty;
                        foreach (var info in pre_work[i])
                        {
                            item_code = info.Key;
                            content = info.Value;
                        }
                        _level = item_code.Count(a => a == '.') + 1;
                        lstCCI.Add(new Sys_Construction_Content_Init
                        {
                            id = Guid.NewGuid(),
                            delete_status = (int)SystemDataStatus.Valid,
                            create_date = nowTime,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            level = _level,
                            item_code = item_code,
                            content = content,
                            line_number = line_number,

                            exp_type = "",
                            exp_value = "",
                            work_type = ConstructionContentWorkTypeEnum.PreWork,
                            point_type = strEmpty,
                            //master_id = null,
                        });
                    }

                    line_number = 0;
                    //对子级数据进行解析并填充到list
                    for (var i = 0; i < pre_work_child.Count; i++)
                    {
                        line_number++;
                        item_code = string.Empty;
                        content = string.Empty;
                        foreach (var info in pre_work_child[i])
                        {
                            item_code = info.Key;
                            content = info.Value;
                        }
                        _level = item_code.Count(a => a == '.') + 1;
                        lstCCI_child.Add(new Sys_Construction_Content_Init
                        {
                            id = Guid.NewGuid(),
                            delete_status = (int)SystemDataStatus.Valid,
                            create_date = nowTime,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            level = _level,
                            item_code = item_code,
                            content = content,
                            line_number = line_number,

                            exp_type = "",
                            exp_value = "",
                            work_type = ConstructionContentWorkTypeEnum.PreWork,
                            point_type = strEmpty,
                            //master_id = null,
                        });
                    }

                    master_id = string.Empty;
                    isExist = false;
                    for (var i = 1; i < lstCCI_child.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(lstCCI_child[i].item_code) && !string.IsNullOrEmpty(lstCCI_child[i].content))
                        {
                            var parent = lstCCI.Where(a => a.item_code == lstCCI_child[i].item_code && a.content == lstCCI_child[i].content).FirstOrDefault();
                            if (parent != null)
                                master_id = parent.id.ToString();
                            continue;
                        }

                        //已录入不再重复录入
                        if (lstCCI.Where(a => a.item_code == lstCCI_child[i].item_code && a.content == lstCCI_child[i].content).ToList().Any())
                            continue;

                        line_number++;
                        lstCCI_child[i].line_number = line_number;
                        lstCCI_child[i].master_id = Guid.Parse(master_id);
                        lstCCI.Add(lstCCI_child[i]);
                    }

                    SetMasterId(lstCCI, ConstructionContentWorkTypeEnum.PreWork);
                }

                nowTime = DateTime.Now;
                //读取Site Work
                sheetIndex = _configuration.GetSection("engineering_details:site_work:sheet_index").Value;
                rowStart = _configuration.GetSection("engineering_details:site_work:row_start").Value;
                var append = _configuration.GetSection("engineering_details:site_work:append").Value;
                var columns = _configuration.GetSection("engineering_details:site_work:columns").Get<List<string>>();
                var dicColumns = _configuration.GetSection("engineering_details:site_work:columns_list").Get<Dictionary<string, List<string>>>();
                var site_work = NPOIHelper.ReadSiteWorkSheet(filePath, int.Parse(sheetIndex.ToString()), int.Parse(rowStart.ToString()), int.Parse(append.ToString()), columns, dicColumns);
                if (site_work != null && site_work.Data != null && site_work.Data.Rows.Count > 0)
                {
                    for (var i = 0; i < site_work.Data.Rows.Count; i++)
                    {
                        var row = site_work.Data.Rows[i];
                        if (int.Parse(append.ToString()) > 1)
                        {
                            quantity = row["Columns_2"].ToString();
                            if (!string.IsNullOrWhiteSpace(quantity))
                                quantity = $" {quantity}";

                            item_code = row["Columns_0"].ToString();
                            content = row["Columns_1"].ToString();
                        }

                        var strChiefInspector = row[columns[0]].ToString();
                        var strManagement = row[columns[1]].ToString();
                        var strMainStaff = row[columns[2]].ToString();
                        var strSecurityDeparment = row[columns[3]].ToString();
                        var strCPWPIC = row[columns[4]].ToString();
                        var strJudge = row[columns[5]].ToString();
                        var strJudgeManagement = row[columns[6]].ToString();
                        var strAuto = row[columns[7]].ToString();

                        lstCCI.Add(new Sys_Construction_Content_Init
                        {
                            id = Guid.NewGuid(),
                            delete_status = (int)SystemDataStatus.Valid,
                            create_date = nowTime,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            level = item_code.Count(a => a == '.') + 1,
                            item_code = item_code,
                            content = $"{content}{quantity}",
                            line_number = (i + 1),
                            extend_attr = "{\"chief\":\"" + strChiefInspector +
                                        "\",\"manager\":\"" + strManagement +
                                        "\",\"main_staff\":\"" + strMainStaff +
                                        "\",\"security_deparment\":\"" + strSecurityDeparment +
                                        "\",\"cp_wpic\":\"" + strCPWPIC +
                                        "\",\"judge\":\"" + strJudge +
                                        "\",\"judge_manage\":\"" + strJudgeManagement +
                                        "\",\"auto\":\"" + strAuto + "\"}",

                            exp_type = "",
                            exp_value = "",
                            work_type = ConstructionContentWorkTypeEnum.SiteWork,
                            point_type = strEmpty,
                            //master_id = null,
                        });
                    }

                    SetMasterId(lstCCI, ConstructionContentWorkTypeEnum.SiteWork);
                }

                nowTime = DateTime.Now;
                //读取Site Survey
                sheetIndex = _configuration.GetSection("engineering_details:site_survey:sheet_index").Value;
                rowStart = _configuration.GetSection("engineering_details:site_survey:row_start").Value;
                var site_survey = NPOIHelper.ReadSiteSurveyAsKeyValuePairs(filePath, int.Parse(sheetIndex.ToString()), int.Parse(rowStart.ToString()));
                if (site_survey != null && site_survey.Count > 0)
                {
                    foreach (var item in site_survey)
                    {
                        item_code = string.Empty;
                        content = string.Empty;
                        foreach (var info in item.Value)
                        {
                            item_code = info.Key;
                            content = info.Value;
                        }

                        lstCCI.Add(new Sys_Construction_Content_Init
                        {
                            id = Guid.NewGuid(),
                            delete_status = (int)SystemDataStatus.Valid,
                            create_date = nowTime,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            level = item_code.Count(a => a == '.') + 1,
                            item_code = item_code,
                            content = content,
                            line_number = item.Key,

                            exp_type = "",
                            exp_value = "",
                            work_type = ConstructionContentWorkTypeEnum.SiteSurvey,
                            point_type = strEmpty,
                            //master_id = null,
                        });
                    }

                    SetMasterId(lstCCI, ConstructionContentWorkTypeEnum.SiteSurvey);
                }

                nowTime = DateTime.Now;
                //读取Sub-C.Work
                sheetIndex = _configuration.GetSection("engineering_details:sub_c_work:sheet_index").Value;
                rowStart = _configuration.GetSection("engineering_details:sub_c_work:row_start").Value;
                var sub_c_work = NPOIHelper.ReadSiteSurveyAsKeyValuePairs(filePath, int.Parse(sheetIndex.ToString()), int.Parse(rowStart.ToString()));
                if (sub_c_work != null && sub_c_work.Count > 0)
                {
                    foreach (var item in sub_c_work)
                    {
                        item_code = string.Empty;
                        content = string.Empty;
                        foreach (var info in item.Value)
                        {
                            item_code = info.Key;
                            content = info.Value;
                        }

                        lstCCI.Add(new Sys_Construction_Content_Init
                        {
                            id = Guid.NewGuid(),
                            delete_status = (int)SystemDataStatus.Valid,
                            create_date = nowTime,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            level = item_code.Count(a => a == '.') + 1,
                            item_code = item_code,
                            content = content,
                            line_number = item.Key,
                            exp_type = "",
                            exp_value = "",

                            work_type = ConstructionContentWorkTypeEnum.SubCWork,
                            point_type = strEmpty,
                            //master_id = null,
                        });
                    }

                    SetMasterId(lstCCI, ConstructionContentWorkTypeEnum.SubCWork);
                }

                //读取T&C

                //读取O&M

                #endregion

                #region  -- 汇总设置external_link_id --

                //设置汇总设置external_link_id
                foreach (var item in lstCCI)
                {
                    if (item.work_type == ConstructionContentWorkTypeEnum.SiteWork || item.work_type == ConstructionContentWorkTypeEnum.PreWork)
                    {
                        _repository.Add(item);
                    }
                    else
                    {
                        if (item.content.IndexOf('/') > -1)
                        {
                            var _content = item.content.Split('/');
                            if (_content.Length > 0)
                                content = _content[1].Trim();
                            else
                                content = item.content.Trim();
                        }
                        else
                            content = item.content.Trim();

                        var list = lstCCI.Where(a => a.work_type == item.work_type && a.content.Contains(content)
                                    && (a.work_type != ConstructionContentWorkTypeEnum.SiteWork
                                        && a.id != item.id
                                        && a.content != item.content
                                        )
                                ).ToList();
                        if (!string.IsNullOrEmpty(item.item_code)
                            && !string.IsNullOrEmpty(item.content)
                            && list.Count > 0)
                        {
                            foreach (var info in list)
                            {
                                //查看item_code
                                var item_code_detail = ExtractKeywordWithVersion(info.content, content);
                                if (!string.IsNullOrEmpty(item_code_detail))
                                {
                                    item_code = item_code_detail.Replace(content + "_", "").Trim();
                                    var model = lstCCI.Where(a => a.work_type == item.work_type && a.item_code == item_code).FirstOrDefault();
                                    if (model != null)
                                        info.external_link_id = model.id;   //跳转链接id
                                }
                            }
                        }

                        _repository.Add(item);
                    }
                }

                #endregion

                // 批量软删除 - 将整表所有未删除的数据改为删除状态
                var validItems = _repository.Find(a => a.delete_status == (int)SystemDataStatus.Valid).ToList();
                foreach (var item in validItems)
                {
                    item.delete_status = (int)SystemDataStatus.Invalid;
                    item.modify_date = nowTime;
                    item.modify_id = UserContext.Current.UserId;
                    item.modify_name = UserContext.Current.UserName;
                }
                _repository.UpdateRange(validItems);

                //批量插入（提交事务）
                _repository.SaveChanges();

                return WebResponseContent.Instance.OK("Ok");
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Construction_Content_Init.ImportData", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #region  -- 私有 --

        private List<Sys_Construction_Content_Init> SetMasterId(List<Sys_Construction_Content_Init> lstCCI, string workType)
        {
            try
            {
                //设置master_id
                var codeIdMap = new Dictionary<string, Dictionary<int, Guid>>();
                Guid? lastNonEmptyCodeId = null;
                string lastNonEmptyItemCode = null;

                var siteWorkItems = lstCCI
                    .Where(a => a.work_type == workType)
                    .OrderBy(item => item.line_number)
                    .ToList();

                // 第一遍扫描：构建完整的item_code到id的映射表
                for (var i = 0; i < siteWorkItems.Count; i++)
                {
                    var item = siteWorkItems[i];
                    if (!string.IsNullOrEmpty(item.item_code))
                    {
                        if (!codeIdMap.ContainsKey(item.item_code))
                        {
                            codeIdMap[item.item_code] = new Dictionary<int, Guid>();
                        }
                        codeIdMap[item.item_code][(int)item.line_number] = (Guid)item.id;
                    }
                }

                // 第二遍扫描：设置master_id
                for (var i = 0; i < siteWorkItems.Count; i++)
                {
                    var item = siteWorkItems[i];
                    // 如果master_id已设置，则不需要处理
                    if (item.master_id.HasValue)
                    {
                        continue;
                    }

                    // 检查item_code是否为空
                    if (string.IsNullOrEmpty(item.item_code))
                    {
                        // 如果item_code为空，使用最近一个非空item_code的ID作为父级
                        if (lastNonEmptyCodeId.HasValue)
                        {
                            item.master_id = lastNonEmptyCodeId.Value;
                        }
                        continue;
                    }

                    // 更新最近非空item_code记录
                    lastNonEmptyCodeId = item.id;
                    lastNonEmptyItemCode = item.item_code;

                    // 根据item_code设置master_id - 支持中间层级缺失情况
                    var codeParts = item.item_code.Split('.');
                    if (codeParts != null && codeParts.Length > 1)
                    {
                        bool parentFound = false;

                        // 尝试查找直接父级（去掉最后一个部分）
                        var parentCodeParts = codeParts.Take(codeParts.Length - 1).ToArray();
                        var directParentCode = string.Join(".", parentCodeParts);

                        if (codeIdMap.ContainsKey(directParentCode))
                        {
                            // 找到直接父级
                            (int __line_number, Guid? __id) = GetDictionary(codeIdMap[directParentCode]);
                            if (__line_number <= item.line_number)
                            {
                                item.master_id = __id;
                            }
                            parentFound = true;
                        }
                        else
                        {
                            // 如果直接父级不存在，尝试逐级向上查找最近的有效父级
                            for (int level = parentCodeParts.Length - 1; level >= 1 && !parentFound; level--)
                            {
                                var ancestorCodeParts = parentCodeParts.Take(level).ToArray();
                                var ancestorItemCode = string.Join(".", ancestorCodeParts);

                                if (codeIdMap.ContainsKey(ancestorItemCode))
                                {
                                    // 修复这里的错误：使用ancestorItemCode而不是directParentCode
                                    (int __line_number, Guid? __id) = GetDictionary(codeIdMap[ancestorItemCode]);
                                    if (__line_number <= item.line_number)
                                    {
                                        item.master_id = __id;
                                    }
                                    parentFound = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Construction_Content_Init.SetMasterId", e);
            }

            return lstCCI;
        }

        private (int, Guid?) GetDictionary(Dictionary<int, Guid> dic)
        {
            int line_number = 0;
            Guid? id = null;
            try
            {
                foreach (var item in dic)
                {
                    line_number = item.Key;
                    id = item.Value;
                }
            }
            catch(Exception e)
            {

            }
            return (line_number, id);
        }

        /// <summary>
        /// 获取当前sheetname当前的item_code最大line_number
        /// </summary>
        /// <param name="work_type"></param>
        /// <param name="item_code"></param>
        /// <returns></returns>
        private int GetLastLinNumber(string work_type, Guid? master_id)
        {
            int res = 0;//预设值

            try
            {
                var query = _repository.FindAsIQueryable(a => a.delete_status == (int)SystemDataStatus.Valid && a.work_type == work_type && !string.IsNullOrEmpty(a.line_number.ToString()));                
                query = query.WhereIF(master_id.HasValue, a => a.master_id == master_id);

                var sql = query.ToQueryString();

                var model = query.OrderByDescending(a => a.line_number).FirstOrDefault();
                if(model != null)
                {
                    res = (int)model.line_number;
                }
            }
            catch(Exception e)
            {
                
            }

            return res;
        }

        /// <summary>
        /// 从文本中提取指定关键词及其后面的版本号部分
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <param name="keyword">关键词（如"Fire Service"）</param>
        /// <returns>提取的关键词加版本号部分，如果未找到则返回null</returns>
        private string ExtractKeywordWithVersion(string text, string keyword)
        {
            try
            {
                if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword))
                {
                    return null;
                }

                // 使用正则表达式匹配关键词后跟下划线、数字和点的模式
                // 例如：Fire Service_2.1.7.1
                string pattern = $@"{Regex.Escape(keyword)}_[\d\.]+";
                Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    // 获取匹配的完整内容（包括后面可能的空格）
                    int startIndex = match.Index;
                    // 查找下一个非空格字符的位置来确定结束位置
                    int endIndex = text.Length;
                    for (int i = match.Index + match.Length; i < text.Length; i++)
                    {
                        if (!char.IsWhiteSpace(text[i]))
                        {
                            endIndex = i;
                            break;
                        }
                    }

                    return text.Substring(startIndex, endIndex - startIndex);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region  -- 已弃用 --

        private List<Sys_Construction_Content_Init> SetMasterId_bak(List<Sys_Construction_Content_Init> lstCCI, string workType)
        {
            try
            {
                //设置master_id
                var codeIdMap = new Dictionary<string, Guid>();
                Guid? lastNonEmptyCodeId = null;
                string lastNonEmptyItemCode = null;

                // 首先筛选出需要处理的项，并按item_code的层级深度和数值顺序排序
                // 确保父级项在子级项之前处理
                var siteWorkItems = lstCCI
                    .Where(a => a.work_type == workType)
                    .OrderBy(item => {
                        if (string.IsNullOrEmpty(item.item_code))
                            return int.MaxValue; // 空item_code最后处理
                        else
                            return item.item_code.Split('.').Length; // 按层级深度排序
                    })
                    .ThenBy(item => {
                        if (string.IsNullOrEmpty(item.item_code))
                            return string.Empty;
                        else
                            return item.item_code; // 按数值排序
                    })
                    .ToList();

                // 重置codeIdMap，确保重新构建映射关系
                codeIdMap = new Dictionary<string, Guid>();

                // 第一遍扫描：构建完整的item_code到id的映射表
                //foreach (var item in siteWorkItems)
                for (var i = 0; i < siteWorkItems.Count; i++)
                {
                    var item = siteWorkItems[i];
                    if (!string.IsNullOrEmpty(item.item_code) && !codeIdMap.ContainsKey(item.item_code))
                    {
                        codeIdMap[item.item_code] = item.id;
                    }
                }

                // 第二遍扫描：设置master_id
                //foreach (var item in siteWorkItems)
                for (var i = 0; i < siteWorkItems.Count; i++)
                {
                    var item = siteWorkItems[i];
                    // 如果master_id已设置，则不需要处理
                    if (item.master_id.HasValue)
                    {
                        continue;
                    }

                    // 检查item_code是否为空
                    if (string.IsNullOrEmpty(item.item_code))
                    {
                        // 如果item_code为空，使用最近一个非空item_code的ID作为父级
                        if (lastNonEmptyCodeId.HasValue)
                        {
                            item.master_id = lastNonEmptyCodeId.Value;
                        }
                        continue;
                    }

                    // 更新最近非空item_code记录
                    lastNonEmptyCodeId = item.id;
                    lastNonEmptyItemCode = item.item_code;

                    // 根据item_code设置master_id - 支持中间层级缺失情况
                    var codeParts = item.item_code.Split('.');
                    if (codeParts != null && codeParts.Length > 1)
                    {
                        bool parentFound = false;

                        // 尝试查找直接父级（去掉最后一个部分）
                        var parentCodeParts = codeParts.Take(codeParts.Length - 1).ToArray();
                        var directParentCode = string.Join(".", parentCodeParts);

                        if (codeIdMap.ContainsKey(directParentCode))
                        {
                            // 找到直接父级
                            item.master_id = codeIdMap[directParentCode];
                            parentFound = true;
                        }
                        else
                        {
                            // 如果直接父级不存在，尝试逐级向上查找最近的有效父级
                            for (int level = parentCodeParts.Length - 1; level >= 1 && !parentFound; level--)
                            {
                                var ancestorCodeParts = parentCodeParts.Take(level).ToArray();
                                var ancestorItemCode = string.Join(".", ancestorCodeParts);

                                if (codeIdMap.ContainsKey(ancestorItemCode))
                                {
                                    item.master_id = codeIdMap[ancestorItemCode];
                                    parentFound = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return lstCCI;
        }

        private List<Sys_Construction_Content_Init> SetMasterId_bak2(List<Sys_Construction_Content_Init> lstCCI, string workType)
        {
            try
            {
                //设置master_id
                var codeIdMap = new Dictionary<string, Dictionary<int, Guid>>();
                Guid? lastNonEmptyCodeId = null;
                string lastNonEmptyItemCode = null;

                // 首先筛选出需要处理的项，并按item_code的层级深度和数值顺序排序
                // 确保父级项在子级项之前处理
                //var siteWorkItems = lstCCI
                //    .Where(a => a.work_type == workType)
                //    .OrderBy(item => {
                //        if (string.IsNullOrEmpty(item.item_code))
                //            return int.MaxValue; // 空item_code最后处理
                //        else
                //            return item.item_code.Split('.').Length; // 按层级深度排序
                //    })
                //    .ThenBy(item => {
                //        if (string.IsNullOrEmpty(item.item_code))
                //            return string.Empty;
                //        else
                //            return item.item_code; // 按数值排序
                //    })
                //    .ToList();

                var siteWorkItems = lstCCI
                    .Where(a => a.work_type == workType)
                    .OrderBy(item => item.line_number)
                    .ToList();

                // 第一遍扫描：构建完整的item_code到id的映射表
                //foreach (var item in siteWorkItems)
                for (var i = 0; i < siteWorkItems.Count; i++)
                {
                    var item = siteWorkItems[i];
                    if (!string.IsNullOrEmpty(item.item_code) && !codeIdMap.ContainsKey(item.item_code))
                    {
                        codeIdMap[item.item_code] = new Dictionary<int, Guid> { { (int)item.line_number, (Guid)item.id } };
                    }
                }

                // 第二遍扫描：设置master_id
                //foreach (var item in siteWorkItems)
                for (var i = 0; i < siteWorkItems.Count; i++)
                {
                    var item = siteWorkItems[i];
                    // 如果master_id已设置，则不需要处理
                    if (item.master_id.HasValue)
                    {
                        continue;
                    }

                    // 检查item_code是否为空
                    if (string.IsNullOrEmpty(item.item_code))
                    {
                        // 如果item_code为空，使用最近一个非空item_code的ID作为父级
                        if (lastNonEmptyCodeId.HasValue)
                        {
                            item.master_id = lastNonEmptyCodeId.Value;
                        }
                        continue;
                    }

                    // 更新最近非空item_code记录
                    lastNonEmptyCodeId = item.id;
                    lastNonEmptyItemCode = item.item_code;

                    // 根据item_code设置master_id - 支持中间层级缺失情况
                    var codeParts = item.item_code.Split('.');
                    if (codeParts != null && codeParts.Length > 1)
                    {
                        bool parentFound = false;

                        // 尝试查找直接父级（去掉最后一个部分）
                        var parentCodeParts = codeParts.Take(codeParts.Length - 1).ToArray();
                        var directParentCode = string.Join(".", parentCodeParts);

                        if (codeIdMap.ContainsKey(directParentCode))
                        {
                            // 找到直接父级
                            (int __line_number, Guid? __id) = GetDictionary(codeIdMap[directParentCode]);
                            if (__line_number > item.line_number)
                            {
                                //如果直接父级的line_number大于当前项的line_number，说明不是父子关系，继续往上找
                            }
                            else item.master_id = __id;

                            parentFound = true;
                        }
                        else
                        {
                            // 如果直接父级不存在，尝试逐级向上查找最近的有效父级
                            for (int level = parentCodeParts.Length - 1; level >= 1 && !parentFound; level--)
                            {
                                var ancestorCodeParts = parentCodeParts.Take(level).ToArray();
                                var ancestorItemCode = string.Join(".", ancestorCodeParts);

                                if (codeIdMap.ContainsKey(ancestorItemCode))
                                {
                                    // 找到直接父级 
                                    (int __line_number, Guid? __id) = GetDictionary(codeIdMap[directParentCode]);
                                    if (__line_number > item.line_number)
                                    {
                                        //如果直接父级的line_number大于当前项的line_number，说明不是父子关系，继续往上找
                                    }
                                    else item.master_id = __id;
                                    parentFound = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return lstCCI;
        }

        #endregion
    }
}

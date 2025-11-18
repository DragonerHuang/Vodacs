
using Dm.util;
using LinqKit;
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
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IRepositories.Biz;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_SiteService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Quotation_SiteRepository _repository;//访问数据库

        private readonly ISys_ContactRepository _contactRepository;          // 联系人仓储
        private readonly ISys_File_RecordsRepository _fileRecordsRepository; // 文件记录仓储

        private readonly ILocalizationService _localizationService;          // 国际化服务
        private readonly ISys_File_RecordsService _fileRecordsService;       // 文件记录服务

        private readonly IBiz_QuotationRepository _quotationRepository;      // 报价仓储
        private readonly IBiz_QuotationService _quotationService;            // 报价服务
        private readonly IBiz_Quotation_DeadlineService _quotationDeadlineService; // 期限管理服务

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_SiteService(
            IBiz_Quotation_SiteRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ISys_ContactRepository contactRepository,
            ISys_File_RecordsRepository fileRecordsRepository,
            ILocalizationService localizationService,
            ISys_File_RecordsService fileRecordsService,
            IBiz_QuotationRepository quotationRepository,
            IBiz_QuotationService quotationService,
            IBiz_Quotation_DeadlineService quotationDeadlineService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _contactRepository = contactRepository;
            _fileRecordsRepository = fileRecordsRepository;
            _localizationService = localizationService;
            _fileRecordsService = fileRecordsService;
            _quotationRepository = quotationRepository;
            _quotationService = quotationService;
            _quotationDeadlineService = quotationDeadlineService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 查询现场考察列表（分页）
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQnSitePageAsync(PageInput<QnSiteSearchDto> pageInput)
        {
            try
            {
                var queryPam = pageInput.search;
                var query = Search(queryPam);

                // 默认排序：按面试时间升序
                if (string.IsNullOrEmpty(pageInput.sort_field))
                {
                    pageInput.sort_field = "create_date";
                    pageInput.sort_type = "asc";
                }
                var result = await query.GetPageResultAsync(pageInput);
                result.data = await GetFinishFiles(result.data);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 查询现场考察列表
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQnSiteAsync(QnSiteSearchDto pageInput)
        {
            try
            {
                var queryPam = pageInput;
                var query = Search(queryPam);

                var result = await query.OrderBy(p => p.create_date).ToListAsync();
                result = await GetFinishFiles(result);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        private IQueryable<QnSiteDataDto> Search(QnSiteSearchDto dtoQuery)
        {
            Expression<Func<Biz_Quotation_Site, Sys_Contact, QnSiteDataDto>> select = (qnSite, contact) => new QnSiteDataDto
            {
                id = qnSite.id,
                qn_id = qnSite.qn_id,


                site_visit_time = qnSite.site_visit_time,
                meeting_point = qnSite.meeting_point,
                site_visit = qnSite.site_visit,
                reply_date = qnSite.reply_date,

                contact_id = qnSite.contact_id,
                contact_cht = contact.name_cht,
                contact_eng = contact.name_eng,

                remark = qnSite.remark,
                create_date = qnSite.create_date,
                create_id = qnSite.create_id,
                create_name = qnSite.create_name
            };
            select = select.BuildExtendSelectExpre();

            var siteData = _repository.FindAsIQueryable(p => p.qn_id == dtoQuery.qn_id && p.delete_status == (int)SystemDataStatus.Valid).AsExpandable().AsNoTracking();
            var contactData = _contactRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid).AsNoTracking();

            // 构建左联查询
            var query = from d in siteData
                        join c in contactData on d.contact_id equals c.id into dc
                        from contact in dc.DefaultIfEmpty()
                        select @select.Invoke(d, contact);

            // 应用查询条件
            if (dtoQuery.time.HasValue)
            {
                var day = dtoQuery.time.Value.Date;
                query = query.Where(p =>
                    p.site_visit_time >= day && p.site_visit_time < day.AddDays(1));
            }

            return query;
        }

        /// <summary>
        /// 获取完成文件集合
        /// </summary>
        /// <param name="lstData"></param>
        /// <returns></returns>
        private async Task<List<QnSiteDataDto>> GetFinishFiles(List<QnSiteDataDto> lstData)
        {
            if (lstData == null || lstData.Count == 0)
            {
                return lstData;
            }

            var lstIds = lstData.Select(p => p.id).Distinct().ToList();
            // 仅查询“现场考察文件”的有效并且已完成上传的记录
            var lstFiles = await _fileRecordsRepository
                .FindAsIQueryable(p =>
                        p.master_id.HasValue &&
                        lstIds.Contains(p.master_id.Value) &&
                        p.file_code == UploadFileCode.Site_Visit_Documents &&
                        p.upload_status == (int)UploadStatus.Finish &&
                        p.delete_status == (int)SystemDataStatus.Valid)
                .AsNoTracking()
                .ToListAsync();

            // 将文件记录按 master_id 分组
            var dicFiles = new Dictionary<Guid, List<Sys_File_Records>>();
            foreach (var file in lstFiles)
            {
                var mid = file.master_id.GetValueOrDefault();
                if (!dicFiles.TryGetValue(mid, out var group))
                {
                    group = new List<Sys_File_Records>();
                    dicFiles[mid] = group;
                }
                group.Add(file);
            }

            // 填充到列表数据中
            foreach (var item in lstData)
            {
                if (dicFiles.TryGetValue(item.id, out var files))
                {
                    item.upload_finish_file_info = files.Select(p => new ContractQnFileDto
                    {
                        file_id = p.id,
                        file_name = p.file_name,
                        file_remark = p.remark,
                        file_size = p.file_size.HasValue ? p.file_size.Value : 0
                    }).ToList();
                }
                else
                {
                    item.upload_finish_file_info = new List<ContractQnFileDto>();
                }
            }

            return lstData;
        }

        /// <summary>
        /// 上传完成文件
        /// </summary>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> UpLoadFinishFile(List<IFormFile> lstFiles)
        {
            try
            {
                if (lstFiles == null || lstFiles.Count == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }

                return await _fileRecordsService.CommonUploadToTempAsync(lstFiles, UploadFileCode.Site_Visit_Documents);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


        /// <summary>
        /// 创建现场考察
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> AddQnSiteAsync(AddQnSiteDto input)
        {
            try
            {
                if (input == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                }

                var data = new Biz_Quotation_Site
                {
                    id = Guid.NewGuid(),
                    qn_id = input.qn_id,

                    site_visit_time = input.site_visit_time,
                    site_visit = input.site_visit,
                    meeting_point = input.meeting_point,
                    contact_id = input.contact_id,

                    remark = input.remark,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now
                };

                _repository.Add(data);

                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == input.qn_id.Value);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                // 处理完成文件（从临时目录移动到正式目录并绑定到现场考察记录）
                if (input.upload_finish_file_info != null && input.upload_finish_file_info.Count > 0)
                {
                    var getRootFolderResult = _quotationService.GetFormalFolder(qnData, UploadFileCode.Site_Visit_Documents);
                    if (!getRootFolderResult.status)
                    {
                        return getRootFolderResult;
                    }
                    var rootFolder = getRootFolderResult.data as string;
                    var moveResult = _fileRecordsService.FinishFilesUploadNoSaveChange(input.upload_finish_file_info, data.id, true, rootFolder, UploadFileCode.Site_Visit_Documents);
                    if (!moveResult.status)
                    {
                        return moveResult;
                    }
                }

                //// 更新期限管理：现场考察
                //var isFinish = input.upload_finish_file_info != null && input.upload_finish_file_info.Any(p => p.file_id.HasValue);
                //var deadlineResult = await _quotationDeadlineService.EditByQAChangeAsync(qnData.id, UpcomingEventsEnum.QnPE, data.id, input.site_visit_time, input.upload_finish_file_info.Count > 0);
                //if (!deadlineResult.status)
                //{
                //    return deadlineResult;
                //}

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
        /// 编辑现场考察
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditQnSiteAsync(EditQnSiteDto input)
        {
            try
            {
                if (input == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_null"));
                }

                // 查询原记录
                var entData = await _repository.FindAsyncFirst(p => p.id == input.id && p.delete_status == (int)SystemDataStatus.Valid);
                if (entData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_item_null"));
                }

                // 更新基础字段
                entData.site_visit_time = input.site_visit_time;
                entData.site_visit = input.site_visit;
                entData.meeting_point = input.meeting_point;
                entData.contact_id = input.contact_id;
                entData.remark = input.remark;

                entData.modify_id = UserContext.Current.UserId;
                entData.modify_name = UserContext.Current.UserName;
                entData.modify_date = DateTime.Now;

                _repository.Update(entData);

                // 获取报价并计算正式目录
                var qnData = await _quotationRepository.FindAsyncFirst(p => p.id == entData.qn_id.Value);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }

                var folderResult = _quotationService.GetFormalFolder(qnData, UploadFileCode.Site_Visit_Documents);
                if (!folderResult.status)
                {
                    return folderResult;
                }
                var rootFolder = folderResult.data as string;

                var fileInfos = input.upload_finish_file_info;
                var uploadIds = fileInfos.Where(p => p.file_id.HasValue).Select(p => p.file_id.Value).ToList();
                var dicUpload = fileInfos.Where(p => p.file_id.HasValue).ToDictionary(p => p.file_id.Value);
                var lstNowRecords = await _fileRecordsRepository.FindAsync(p => uploadIds.Contains(p.id));
                var dicNowRecords = lstNowRecords.ToDictionary(p => p.id);
                var lstEditRecords = new List<Sys_File_Records>();

                // 处理文件记录
                foreach (var item in lstNowRecords)
                {
                    var boolGet = dicUpload.TryGetValue(item.id, out var dtoFile);
                    if (!boolGet)
                    {
                        continue;
                    }
                    // 没masterid的是新增的
                    if (!item.master_id.HasValue)
                    {
                        if (item.upload_status == (int)UploadStatus.Upload)
                        {
                            item.upload_status = (int)UploadStatus.Finish;
                            // 这个需要移动文件
                            item.file_path = _fileRecordsService.MoveFile(rootFolder, item);
                        }
                        item.master_id = entData.id;
                        item.modify_id = UserContext.Current.UserInfo.User_Id;
                        item.modify_name = UserContext.Current.UserInfo.UserName;
                        item.modify_date = DateTime.Now;
                        item.remark = dtoFile.file_remark;
                        lstEditRecords.add(item);
                        continue;
                    }

                    if (item.remark != dtoFile.file_remark)
                    {
                        item.modify_id = UserContext.Current.UserInfo.User_Id;
                        item.modify_name = UserContext.Current.UserInfo.UserName;
                        item.modify_date = DateTime.Now;
                        item.remark = dtoFile.file_remark;
                        lstEditRecords.add(item);
                        continue;
                    }
                }

                // 获取之前存储的文件记录(用来删除的)
                var lstBeforeRecords = await _fileRecordsRepository.FindAsync(p => p.delete_status == (int)SystemDataStatus.Valid &&
                                                                                   p.file_code == UploadFileCode.Site_Visit_Documents &&
                                                                                   p.master_id == entData.id &&
                                                                                   !uploadIds.Contains(p.id));
                foreach (var old in lstBeforeRecords)
                {
                    old.delete_status = (int)SystemDataStatus.Invalid;
                    old.modify_id = UserContext.Current.UserInfo.User_Id;
                    old.modify_name = UserContext.Current.UserInfo.UserName;
                    old.modify_date = DateTime.Now;
                    lstEditRecords.Add(old);
                }
                _fileRecordsRepository.UpdateRange(lstEditRecords);

                // 更新期限管理：招标面试
                //var isFinish = input.upload_finish_file_info != null && input.upload_finish_file_info.Any(p => p.file_id.HasValue);
                //var deadlineResult = await _quotationDeadlineService
                //    .EditByQAChangeAsync(entData.qn_id.Value, UpcomingEventsEnum.QnPE, entData.id, input.site_visit_time, isFinish);
                //if (!deadlineResult.status)
                //{
                //    return deadlineResult;
                //}

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
        /// 删除现场考察
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteQnSiteAsync(Guid id)
        {
            try
            {
                // 查询原记录
                var entData = await _repository.FindAsyncFirst(p => p.id == id && p.delete_status == (int)SystemDataStatus.Valid);
                if (entData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_item_null"));
                }
                entData.delete_status = (int)SystemDataStatus.Invalid;
                entData.modify_id = UserContext.Current.UserId;
                entData.modify_name = UserContext.Current.UserName;
                entData.modify_date = DateTime.Now;
                _repository.Update(entData);

                // 删除期限管理
                //await _quotationDeadlineService.DeleteAsync(entData.qn_id.Value, entData.id, UpcomingEventsEnum.QnPE);

                // 删除文件
                var files = await _fileRecordsRepository
                    .FindAsync(p => p.master_id == entData.id && p.file_code == UploadFileCode.Site_Visit_Documents && p.delete_status == (int)SystemDataStatus.Valid);
                _fileRecordsService.DeleteUploadFiles(files);

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
        /// 修改回复日期
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditReplyDateAsync(QnSiteSearchDto input)
        {
            try
            {
                var siteData = await _repository.FindAsync(p => p.qn_id == input.qn_id && p.delete_status == (int)SystemDataStatus.Valid);
                foreach (var item in siteData)
                {
                    item.reply_date = input.time;

                    item.modify_id = UserContext.Current.UserId;
                    item.modify_name = UserContext.Current.UserName;
                    item.modify_date = DateTime.Now;
                }
                _repository.UpdateRange(siteData);

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    } 
}

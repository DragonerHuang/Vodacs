
using Dm.util;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.TermStore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
    public partial class Biz_Quotation_DeadlineService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Quotation_DeadlineRepository _repository;//访问数据库

        private readonly ISys_DictionaryListRepository _dictionaryListRepository;   // 数字字典列表仓储
        private readonly ISys_DictionaryRepository _dictionaryRepository;           // 数字字典仓储
        private readonly IBiz_QuotationRepository _quotationRepository;             // 报价仓储
        private readonly IBiz_ContractRepository _contractRepository;               // 合同仓储
        private readonly ISys_ContactRepository _contactRepository;                 // 联系人仓储
        private readonly IBiz_Upcoming_EventsRepository _upcomingEventsRepository;  // 事件仓储
        private readonly IBiz_Quotation_HeadRepository _quotationHeadRepository;    // 负责人仓储

        private readonly ILocalizationService _localizationService;                 // 国际化
        private readonly Biz_Upcoming_EventsService _eventsService;                 // 事件服务

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_DeadlineService(
            IBiz_Quotation_DeadlineRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ISys_DictionaryListRepository dictionaryListRepository,
            ISys_DictionaryRepository dictionaryRepository,
            ILocalizationService localizationService,
            IBiz_QuotationRepository quotationRepository,
            IBiz_ContractRepository contractRepository,
            ISys_ContactRepository contactRepository,
            Biz_Upcoming_EventsService eventsService,
            IBiz_Upcoming_EventsRepository upcomingEventsRepository,
            IBiz_Quotation_HeadRepository quotationHeadRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _dictionaryListRepository = dictionaryListRepository;
            _dictionaryRepository = dictionaryRepository;
            _localizationService = localizationService;
            _quotationRepository = quotationRepository;
            _contractRepository = contractRepository;
            _contactRepository = contactRepository;
            _eventsService = eventsService;
            _upcomingEventsRepository = upcomingEventsRepository;
            _quotationHeadRepository = quotationHeadRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDeadLineListPageAsync(PageInput<QnDeadlineSearchDto> dtoQuery)
        {
            try
            {
                var queryPam = dtoQuery.search;

                var query = Search(queryPam);

                // 执行分页查询
                if (string.IsNullOrEmpty(dtoQuery.sort_field))
                {
                    dtoQuery.sort_field = "create_date";
                    dtoQuery.sort_type = "asc";
                }

                var result = await query.GetPageResultAsync(dtoQuery);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 查询列表（不分页）
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDeadLineListAsync(QnDeadlineSearchDto dtoQuery)
        {
            try
            {
                var query = Search(dtoQuery);
                var result = await query.ToListAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 查询内容，构建表达式
        /// </summary>
        /// <param name="dtoQuery"></param>
        /// <returns></returns>
        private IQueryable<QnDeadlineDto> Search(QnDeadlineSearchDto dtoQuery)
        {
            Expression<Func<Biz_Quotation_Deadline, Sys_Contact, QnDeadlineDto>> select = (qn_deadline, contact) => new QnDeadlineDto
            {
                id = qn_deadline.id,
                qn_id = qn_deadline.qn_id,
                term_type = qn_deadline.term_type,
                handler_id = qn_deadline.handler_id,
                handler_cht = contact.name_cht,
                handler_eng = contact.name_eng,
                is_complete = qn_deadline.is_complete,
                closing_date = qn_deadline.closing_date,
                create_date = qn_deadline.create_date,
                create_id = qn_deadline.create_id,
                create_name = qn_deadline.create_name,
                complete_date = qn_deadline.complete_date,
                exp_complete_date = qn_deadline.exp_complete_date
            };
            select = select.BuildExtendSelectExpre();

            // 获取截止日期数据和联系人数据
            var deadlineData = _repository.FindAsIQueryable(p => p.qn_id == dtoQuery.qn_id && p.delete_status == (int)SystemDataStatus.Valid).AsExpandable().AsNoTracking();
            var contactData = _contactRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid).AsNoTracking();

            // 构建左联查询
            var query = from d in deadlineData
                        join c in contactData on d.handler_id equals c.id into dc
                        from contact in dc.DefaultIfEmpty()
                        select @select.Invoke(d, contact);

            // 应用查询条件
            if (!string.IsNullOrEmpty(dtoQuery.deadline_type))
            {
                query = query.Where(q => q.term_type == dtoQuery.deadline_type);
            }

            return query;
        }

        /// <summary>
        /// 编辑期限管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditDealLineAsync(EditQnDeadlineDto input)
        {
            try
            {
                // 查找要编辑的期限记录
                var deadlineData = await _repository.FindFirstAsync(p => p.id == input.id);
                if (deadlineData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("deadline_not_exist"));
                }

                // 更新期限记录
                deadlineData.exp_complete_date = input.exp_complete_date;

                deadlineData.modify_id = UserContext.Current.UserInfo.User_Id;
                deadlineData.modify_name = UserContext.Current.UserInfo.UserName;
                deadlineData.modify_date = DateTime.Now;

                _repository.Update(deadlineData);

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        #region 因数据变化,而改动内容

        /// <summary>
        /// 报价新增时创建截止时间（不执行savechange）
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="contractType"></param>
        /// <param name="closingTime"></param>
        /// <returns></returns>
        public WebResponseContent AddDeadlineByQnAdd(Guid qnId, string contractType, DateTime? closingTime, Guid? relationId)
        {
            try
            {
                var lstDeadlineRecord = new List<Biz_Quotation_Deadline>();

                if (contractType == "Advertisement")
                {
                    lstDeadlineRecord.Add(CreateDeadLine(qnId, "Advertisement", closingTime, relationId));
                    lstDeadlineRecord.Add(CreateDeadLine(qnId, "Pre-qualification"));
                }

                if (contractType == "Preliminary Enquiry Invitation (PEI)")
                {
                    lstDeadlineRecord.Add(CreateDeadLine(qnId, "Preliminary Enquiry（PEI）", closingTime, relationId));
                }

                lstDeadlineRecord.Add(CreateDeadLine(qnId, "Tender"));
                //lstDeadlineRecord.Add(CreateDeadLine(qnId, "Sitevisit"));

                if (lstDeadlineRecord.Count > 0)
                {
                    _repository.AddRange(lstDeadlineRecord);
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
        }

        /// <summary>
        /// 构建期限记录
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="deadlineType"></param>
        /// <param name="closingTime"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        private Biz_Quotation_Deadline CreateDeadLine(Guid qnId, string deadlineType, DateTime? closingTime = null, Guid? relationId = null)
        {
            return new Biz_Quotation_Deadline
            {
                id = Guid.NewGuid(),
                delete_status = (int)SystemDataStatus.Valid,
                create_id = UserContext.Current.UserInfo.User_Id,
                create_name = UserContext.Current.UserInfo.UserName,
                create_date = DateTime.Now,
                qn_id = qnId,
                term_type = deadlineType,
                is_complete = 0,
                closing_date = closingTime,
                relation_id = relationId
            };
        }

        /// <summary>
        /// 因换负责人而改变期限的负责人（不执行savechange）
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="newContactId"></param>
        /// <param name="oldContactId"></param>
        /// <param name="headlerType"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditByHeadlerChangeAsync(Guid qnId, Guid newContactId, Guid? oldContactId, string headlerType)
        {
            try
            {
                if (newContactId == oldContactId)
                {
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }

                // 查找要编辑的期限记录
                var lstDeadlines = await _repository.FindAsync(p => p.qn_id == qnId && p.handler_id == oldContactId);
                if (lstDeadlines.Count == 0)
                {
                    // 没找到就算了
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                var lstDeadlineIds = new List<Guid?>();
                foreach (var item in lstDeadlines)
                {
                    var dat = QnHeadTypeHelper.GetDeadlineTypeByHeadlerType(item.term_type);
                    if (QnHeadTypeHelper.GetDeadlineTypeByHeadlerType(item.term_type) != headlerType)
                    {
                        continue;
                    }
                    lstDeadlineIds.add(item.id);
                    item.handler_id = newContactId;
                    item.modify_id = UserContext.Current.UserInfo.User_Id;
                    item.modify_name = UserContext.Current.UserInfo.UserName;
                    item.modify_date = DateTime.Now;
                }

                _repository.UpdateRange(lstDeadlines);

                // 处理事件
                var lstEvent = await _upcomingEventsRepository
                    .FindAsync(p => lstDeadlineIds.Contains(p.relation_id) && p.recipient_user_id == oldContactId && p.delete_status == (int)SystemDataStatus.Valid);

                foreach (var item in lstEvent)
                {
                    item.recipient_user_id = newContactId;
                    item.modify_id = UserContext.Current.UserInfo.User_Id;
                    item.modify_name = UserContext.Current.UserInfo.UserName;
                    item.modify_date = DateTime.Now;
                }

                _upcomingEventsRepository.UpdateRange(lstEvent);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
           
        }

        /// <summary>
        /// 因合同资料/标书资料保存而改变期限记录的截止时间和完成时间
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <param name="eventsEnum">期限类型</param>
        /// <param name="relationId">管理表id</param>
        /// <param name="closingTime">截止时间</param>
        /// <param name="isFinish">是否完成</param>
        /// <returns></returns>
        public async Task<WebResponseContent> EidtByContractDetailsChangeAsync(
            Guid qnId,
            UpcomingEventsEnum eventsEnum, 
            Guid relationId, 
            DateTime? closingTime = null, 
            bool isFinish = false)
        {
            try
            {
                var deadlineType = CommonHelper.GetUpcomingEventsStrEng((int)eventsEnum);
                var intComplete = isFinish ? 1 : 0;

                var deadlineData =  await _repository
                    .FindAsyncFirst(p => p.qn_id == qnId && p.term_type == deadlineType && p.delete_status == (int)SystemDataStatus.Valid);

                return await SaveDeadline(deadlineData, qnId, deadlineType, eventsEnum, relationId, closingTime, intComplete);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 因问答保存而改变期限记录的截止时间和完成时间
        /// </summary>
        /// <param name="qnId">报价id</param>
        /// <param name="eventsEnum">期限类型</param>
        /// <param name="relationId">管理表id</param>
        /// <param name="closingTime">截止时间</param>
        /// <param name="isFinish">是否完成</param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditByQAChangeAsync(
            Guid qnId,
            UpcomingEventsEnum eventsEnum,
            Guid relationId,
            DateTime? closingTime = null,
            bool isFinish = false)
        {
            try
            {
                var deadlineType = CommonHelper.GetUpcomingEventsStrEng((int)eventsEnum);
                var intComplete = isFinish ? 1 : 0;

                var deadlineData = eventsEnum == UpcomingEventsEnum.QnPQ ?
                    await _repository.FindAsyncFirst(p => p.qn_id == qnId && p.term_type == deadlineType && p.delete_status == (int)SystemDataStatus.Valid) :
                    await _repository.FindAsyncFirst(p => p.qn_id == qnId && p.term_type == deadlineType && p.delete_status == (int)SystemDataStatus.Valid && p.relation_id == relationId);

                return await SaveDeadline(deadlineData, qnId, deadlineType, eventsEnum, relationId, closingTime, intComplete);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 因提交完成文件而改变期限记录的完成时间
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="eventsEnum"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditBySumbitFileAsync(Guid qnId, UpcomingEventsEnum eventsEnum)
        {
            try
            {
                var deadlineType = CommonHelper.GetUpcomingEventsStrEng((int)eventsEnum);

                var deadlineData = await _repository.FindAsyncFirst(p => p.qn_id == qnId && p.term_type == deadlineType && p.delete_status == (int)SystemDataStatus.Valid);
                if (deadlineData != null)
                {
                    return await SaveDeadline(
                        deadlineData, 
                        qnId, 
                        deadlineType, 
                        eventsEnum, 
                        deadlineData.relation_id.HasValue ? deadlineData.relation_id.Value : Guid.Empty, 
                        deadlineData.closing_date, 
                        1);
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 删除期限管理
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="relationId"></param>
        /// <param name="eventsEnum"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteAsync(Guid qnId, Guid relationId, UpcomingEventsEnum eventsEnum)
        {
            try
            {
                var deadlineType = CommonHelper.GetUpcomingEventsStrEng((int)eventsEnum);

                var deadlineData = await _repository
                   .FindAsyncFirst(p => p.qn_id == qnId && p.relation_id == relationId && p.term_type == deadlineType && p.delete_status == (int)SystemDataStatus.Valid);

                if (deadlineData != null) 
                {
                    deadlineData.delete_status = (int)SystemDataStatus.Invalid;
                    deadlineData.modify_id = UserContext.Current.UserId;
                    deadlineData.modify_name = UserContext.Current.UserName;
                    deadlineData.modify_date = DateTime.Now;
                    _repository.Update(deadlineData);

                    await DeleteEventAsync(relationId, eventsEnum);
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 保存期限记录
        /// </summary>
        /// <param name="deadlineData">期限记录</param>
        /// <param name="qnId">报价id</param>
        /// <param name="deadlineType">期限类型</param>
        /// <param name="eventsEnum">期限类型对应的事件类型</param>
        /// <param name="relationId">管理表id</param>
        /// <param name="closingTime">结束时间</param>
        /// <param name="intComplete">是否完成(0:否，1是)</param>
        /// <returns></returns>
        private async Task<WebResponseContent> SaveDeadline(
            Biz_Quotation_Deadline deadlineData, 
            Guid qnId,
            string deadlineType,
            UpcomingEventsEnum eventsEnum,
            Guid relationId,
            DateTime? closingTime,
            int intComplete)
        {
            try
            {
                if (deadlineData == null)
                {
                    // 没找到则创建一个
                    var headData = await _quotationHeadRepository
                        .FindAsIQueryable(p => p.qn_id == qnId && 
                                               p.handler_type == QnHeadTypeHelper.GetQnHeadTypeByDeadlineType(eventsEnum) && 
                                               p.delete_status == (int)SystemDataStatus.Valid)
                        .FirstOrDefaultAsync();
                    return await AddDeadLineAsync(new AddQnDeadlineDto
                    {
                        qn_id = qnId,
                        term_type = deadlineType,
                        handler_id = headData?.handler_id,
                        closing_date = closingTime,
                        is_complete = intComplete,
                        relation_id = relationId
                    });
                }

                if (deadlineData.closing_date == closingTime && deadlineData.is_complete == intComplete)
                {
                    if (deadlineData.relation_id != relationId)
                    {
                        deadlineData.relation_id = relationId;
                        _repository.Update(deadlineData);
                    }
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                
                return await EditDeadlineAsync(deadlineData, closingTime, intComplete);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 创建期限（不执行savechange）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> AddDeadLineAsync(AddQnDeadlineDto input)
        {
            try
            {
                // 报价、合同
                var qnData = await _quotationRepository.FindFirstAsync(p => p.id == input.qn_id);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == qnData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 创建新的期限记录
                var deadlineData = new Biz_Quotation_Deadline
                {
                    id = Guid.NewGuid(),
                    qn_id = input.qn_id,
                    term_type = input.term_type,
                    handler_id = input.handler_id,
                    closing_date = input.closing_date,
                    is_complete = input.is_complete ?? 0, // 默认未完成
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                    relation_id = input.relation_id,
                    complete_date = input.is_complete == 0 ? null : DateTime.Now,
                };

                if (deadlineData.handler_id.HasValue && deadlineData.closing_date.HasValue && deadlineData.is_complete == 0)
                {
                    var eventEnum = CommonHelper.GetUpcomingEventsEnum(deadlineData.term_type);
                    AddEvent(
                        eventEnum,
                        deadlineData.id,
                        qnData.qn_no,
                        ctrData.contract_no,
                        deadlineData.closing_date.Value,
                        deadlineData.handler_id.Value,
                        ctrData.name_eng
                    );
                }

                await _repository.AddAsync(deadlineData);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 编辑期限（不执行savechange）
        /// </summary>
        /// <param name="deadlineData">期限记录</param>
        /// <param name="newTime">修改的时间</param>
        /// <param name="newComplete">修改是否已完成（0：否；1：完成）</param>
        /// <returns></returns>
        private async Task<WebResponseContent> EditDeadlineAsync(Biz_Quotation_Deadline deadlineData, DateTime? newTime, int? newComplete)
        {
            try
            {
                var oldTime = deadlineData.closing_date;
                var oldComplete = deadlineData.is_complete;
                var eventEnum = CommonHelper.GetUpcomingEventsEnum(deadlineData.term_type);

                if (newComplete == 1 && oldComplete != 1)
                {
                    await FinishEventAsync(deadlineData.id, eventEnum);
                }
                else
                {
                    if (!newTime.HasValue)
                    {
                        await DeleteEventAsync(deadlineData.id, eventEnum);
                    }
                    else if (newTime != oldTime && deadlineData.handler_id.HasValue)
                    {
                        // 报价、合同
                        var qnData = await _quotationRepository.FindFirstAsync(p => p.id == deadlineData.qn_id);
                        if (qnData == null)
                        {
                            return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                        }
                        var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == qnData.contract_id);
                        if (ctrData == null)
                        {
                            return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                        }

                        await EditEventAsync(
                            eventEnum,
                            deadlineData.id,
                            qnData.qn_no,
                            ctrData.contract_no,
                            newTime.Value,
                            deadlineData.handler_id.Value,
                            ctrData.name_eng
                        );
                    }
                }

                deadlineData.closing_date = newTime;
                deadlineData.is_complete = newComplete;
                deadlineData.complete_date = newComplete == 0 ? null : DateTime.Now;


                _repository.Update(deadlineData);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// 添加event数据
        /// </summary>
        /// <param name="eventEnum">事件类型</param>
        /// <param name="relationId">关系id，关联到那个id</param>
        /// <param name="number">活动编码</param>
        /// <param name="contractNo">合同编码</param>
        /// <param name="closing_date">截止时间</param>
        /// <param name="eventName">时间名称</param>
        private void AddEvent(
            UpcomingEventsEnum eventEnum, 
            Guid relationId, 
            string number, 
            string contractNo, 
            DateTime closing_date, 
            Guid headerId,
            string eventName)
        {
            var dtoEvent = new EventQnDto
            {
                qn_no = contractNo,
                qn_type = (int)eventEnum,
                closing_date = closing_date.ToString("yyyy-MM-dd")
            };
            string strMsg = JsonConvert.SerializeObject(dtoEvent);
            _eventsService.Add(new Upcoming_Events_OptionDto
            {
                event_name = $"（{CommonHelper.GetUpcomingEventsStr((int)eventEnum)}）" + eventName,
                event_name_eng = $"（{CommonHelper.GetUpcomingEventsStrEng((int)eventEnum)}）" + eventName,
                event_no = number,
                closing_date = closing_date,
                event_type = (int)eventEnum,
                relation_id = relationId,
                remark = strMsg,
                recipient_user_id = headerId,
            });
        }

        /// <summary>
        /// 删除Event
        /// </summary>
        /// <param name="relationId">关系id，关联到那个id</param>
        /// <param name="eventEnum">事件类型</param>
        private async Task DeleteEventAsync(Guid relationId, UpcomingEventsEnum eventEnum)
        {
            await _eventsService.DelAsync(relationId, eventEnum);
        }

        /// <summary>
        /// 编辑event数据
        /// </summary>
        /// <param name="eventEnum">事件类型</param>
        /// <param name="relationId">关系id，关联到那个id</param>
        /// <param name="number">活动编码</param>
        /// <param name="contractNo">合同编码</param>
        /// <param name="closing_date">截止时间</param>
        /// <param name="eventName">时间名称</param>
        private async Task EditEventAsync(
            UpcomingEventsEnum eventEnum,
            Guid relationId,
            string number,
            string contractNo,
            DateTime closing_date,
            Guid headerId,
            string eventName)
        {
            var dtoEvent = new EventQnDto
            {
                qn_no = contractNo,
                qn_type = (int)eventEnum,
                closing_date = closing_date.ToString("yyyy-MM-dd")
            };
            string strMsg = JsonConvert.SerializeObject(dtoEvent);

            var eventId = await _eventsService.GetRelationIDAsync(relationId, eventEnum);
            if (eventId != Guid.Empty)
            {
                await _eventsService.EditAsync(new Upcoming_EventsDto
                {
                    id = eventId,
                    event_name = $"（{CommonHelper.GetUpcomingEventsStr((int)eventEnum)}）" + eventName,
                    event_name_eng = $"（{CommonHelper.GetUpcomingEventsStrEng((int)eventEnum)}）" + eventName,
                    event_no = number,
                    closing_date = closing_date,
                    event_type = (int)eventEnum,
                    relation_id = relationId,
                    remark = strMsg,
                    recipient_user_id = headerId
                });
            }
            else
            {
                AddEvent(eventEnum, relationId, number, contractNo, closing_date, headerId, eventName);
            }
              
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <param name="relationId">关系id，关联到那个id</param>
        /// <param name="eventEnum">事件类型</param>
        /// <returns></returns>
        private async Task FinishEventAsync(Guid relationId, UpcomingEventsEnum eventEnum)
        {
            var eventId = await _eventsService.GetRelationIDAsync(relationId, eventEnum);
            if (eventId != Guid.Empty)
            {
                await _eventsService.FinishAsync(eventId);
            }
        }



        #endregion

        #region 旧代码

        /// <summary>
        /// 获取期限类型下拉列表
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDeadlineTypeAsync(Guid qnId)
        {
            try
            {
                // 报价、合同
                var qnData = await _quotationRepository.FindFirstAsync(p => p.id == qnId);
                if (qnData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_null"));
                }
                var ctrData = await _contractRepository.FindAsyncFirst(p => p.id == qnData.contract_id);
                if (ctrData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));
                }

                // 期限类型
                var lstMainDic = await _dictionaryRepository.FindFirstAsync(p => p.dic_no == "deadline" && p.delete_status == 0);
                if (lstMainDic == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("dic_null"));
                }

                var lstSubDic = await _dictionaryListRepository
                    .WhereIF(true, p => p.dic_id == lstMainDic.dic_id && p.delete_status == 0)
                    .OrderBy(p => p.order_no)
                    .ToListAsync();

                var lstStutsDto = new List<ContractStatusDto>();
                var lstExclude = ExcludeDeadlineType(ctrData.tender_type);
                foreach (var item in lstSubDic)
                {
                    if (lstExclude.Contains(item.dic_value))
                    {
                        continue;
                    }

                    lstStutsDto.Add(new ContractStatusDto
                    {
                        id = item.dic_list_id,
                        index = item.order_no,
                        code = item.dic_value,
                        value = item.dic_name,
                        //value_eng = item.dic_value,
                    });
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstStutsDto);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 根据状态排除项
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        private List<string> ExcludeDeadlineType(string contractType)
        {
            /*
                    第一组
                    1.公開招标Advertisement （必有）
                    2.預審Pre-qualification（必有）
                    3.預審問答Pre-qualification Q&A（视乎情况）
                    -----假设以上阶段审核通过就会进入邀请报价阶段----------
                    4.招标 Tender（必有）
                    5.现场考察 Sitevisit（可同時多個，有情况是分开2天）（必有）
                    6.招标問答 Tender Q&A（视乎情况）
                    7.面試 Tender Interview（视乎情况）

                    第二组
                    1.邀請招标Preliminary Enquiry（PEI）（必有）
                    2.招标 Tender（必有）
                    3.现场考察 Sitevisit（可同時多個，有情况是分开2天）（必有）
                    4.招标問答 Tender Q&A（视乎情况）
                    5.面試 Tender Interview（视乎情况）
             */

            List<string> lstExculde = new List<string>();
            if (contractType == "Advertisement")
            {
                lstExculde.Add("Preliminary Enquiry（PEI）");
            }
            else if (contractType == "Preliminary Enquiry Invitation (PEI)")
            {
                lstExculde.Add("Advertisement");
                lstExculde.Add("Pre-qualification");
                lstExculde.Add("Pre-qualification Q&A");
            }
            else
            {
                lstExculde.Add("Preliminary Enquiry（PEI）");
                lstExculde.Add("Advertisement");
                lstExculde.Add("Pre-qualification");
                lstExculde.Add("Pre-qualification Q&A");
            }

            return lstExculde;
        }


        /// <summary>
        /// 删除期限管理（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteDeadlineAsync(Guid id)
        {
            try
            {
                // 查找要删除的期限记录
                var deadlineData = await _repository.FindFirstAsync(p => p.id == id);
                if (deadlineData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("deadline_not_exist"));
                }

                /*
                 * 以下状态一定要留有一个
                 * 公開招标Advertisement （必有）
                 * 預審Pre-qualification（必有）
                 * 招标 Tender（必有）
                 * 现场考察 Sitevisit（可同時多個，有情况是分开2天）（必有） 
                 * 邀請招标Preliminary Enquiry（PEI）（必有）
                 */

                var isOk = await IsTypeChange(deadlineData.id, deadlineData.qn_id.Value, deadlineData.term_type);
                if (!isOk)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("deadline_not_delete"));
                }

                // 执行软删除
                deadlineData.delete_status = (int)SystemDataStatus.Invalid;
                deadlineData.modify_id = UserContext.Current.UserInfo.User_Id;
                deadlineData.modify_name = UserContext.Current.UserInfo.UserName;
                deadlineData.modify_date = DateTime.Now;

                _repository.Update(deadlineData);

                var eventEnum = CommonHelper.GetUpcomingEventsEnum(deadlineData.term_type);
                await DeleteEventAsync(deadlineData.id, eventEnum);

                await _repository.SaveChangesAsync();

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 一定要有的类型
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsTypeChange(Guid id, Guid qnId, string deadlineType)
        {
            /*
             * 以下状态一定要留有一个
             * 公開招标Advertisement （必有）
             * 預審Pre-qualification（必有）
             * 招标 Tender（必有）
             * 现场考察 Sitevisit（可同時多個，有情况是分开2天）（必有） 
             * 邀請招标Preliminary Enquiry（PEI）（必有）
             */
            var lstMust = new List<string>
            {
                "Advertisement",
                "Pre-qualification",
                "Pre-Tender",
                "Sitevisit",
                "Preliminary Enquiry（PEI）",
                "Tender"
            };
            if (lstMust.Contains(deadlineType))
            {
                return await _repository.ExistsAsync(p => p.qn_id == qnId &&
                                                          p.term_type == deadlineType &&
                                                          p.id != id &&
                                                          p.delete_status == (int)SystemDataStatus.Valid);
            }

            return true;
        }

        #endregion
    }

}
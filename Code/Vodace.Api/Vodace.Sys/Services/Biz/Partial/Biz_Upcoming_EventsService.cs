
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
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
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Biz_Upcoming_EventsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Upcoming_EventsRepository _repository;//访问数据库
        private readonly ISys_User_NewRepository _User_NewRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Upcoming_EventsService(
            IBiz_Upcoming_EventsRepository dbRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
,
            ISys_User_NewRepository user_NewRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            _User_NewRepository = user_NewRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public WebResponseContent GetEventList()
        {
            var contactId = _User_NewRepository.Find(d => d.user_id == UserContext.Current.UserId).FirstOrDefault()?.contact_id;
            if (contactId == null || contactId == Guid.Empty) 
            {
                return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
            }
            var data = repository.Find(d => d.delete_status == (int)SystemDataStatus.Valid && d.recipient_user_id == contactId).Select(d => new UpcomingEventsOut
            {
                id = d.id,
                event_name = d.event_name,
                event_name_eng= d.event_name_eng,
                event_name_chw = ChineseConverter.Convert(d.event_name, ChineseConversionDirection.SimplifiedToTraditional),
                event_no = d.event_no,
                relation_id = d.relation_id,
                closing_date = d.closing_date,
                event_type = d.event_type,
                remark = d.remark,
                days_left_to_close = CommonHelper.DiffDays(d.closing_date)

            }).OrderBy(d => d.closing_date).Take(100).ToList();
            return WebResponseContent.Instance.OK("Ok", data);
        }


        /// <summary>
        /// 添加即将到来的事件，调用的时候特别注意需要执行SaveChange()
        /// </summary>
        /// <param name="upcomingEventDto">事件数据传输对象</param>
        /// <returns>
        /// **********因未进行事务的提交，所以调用的时候特别注意需要执行SaveChange()**********
        /// </returns>
        public WebResponseContent Add(Upcoming_Events_OptionDto dtoUpcomingEventsOption)
        {
            try
            {
                // 参数验证
                if (dtoUpcomingEventsOption == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("event_null"));

                Biz_Upcoming_Events biz_Upcoming_Events = _mapper.Map<Biz_Upcoming_Events>(dtoUpcomingEventsOption);
                biz_Upcoming_Events.days_left_to_close = CommonHelper.DiffDays(dtoUpcomingEventsOption.closing_date);
                biz_Upcoming_Events.delete_status = (int)SystemDataStatus.Valid;
                biz_Upcoming_Events.id = Guid.NewGuid();
                biz_Upcoming_Events.create_date = DateTime.Now;
                biz_Upcoming_Events.create_id = UserContext.Current.UserId;
                biz_Upcoming_Events.create_name = UserContext.Current.UserName;

                _repository.Add(biz_Upcoming_Events);
                return WebResponseContent.Instance.OK("OK", biz_Upcoming_Events);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_Add Exception：{e.Message}", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改即将到来的事件，调用的时候特别注意需要执行SaveChange()
        /// </summary>
        /// <param name="upcomingEventDto">事件数据传输对象</param>
        /// <returns>操作结果</returns>
        /// <returns>
        /// **********因未进行事务的提交，所以调用的时候特别注意需要执行SaveChange()**********
        /// 
        /// </returns>
        public WebResponseContent Edit(Upcoming_EventsDto dtoUpcomingEvents)
        {
            try
            {
                // 参数验证
                if (dtoUpcomingEvents == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("event_null"));

                // 使用事务修改数据
                WebResponseContent result = _repository.DbContextBeginTransaction(() =>
                {
                    // 查询要修改的实体
                    Biz_Upcoming_Events existingEvent = _repository.Find(p => p.id == dtoUpcomingEvents.id && p.delete_status == 0).FirstOrDefault();

                    if (existingEvent == null)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));

                    existingEvent.event_no = dtoUpcomingEvents.event_no;
                    existingEvent.event_name = dtoUpcomingEvents.event_name;
                    existingEvent.event_name_eng = dtoUpcomingEvents.event_name_eng;
                    existingEvent.closing_date = dtoUpcomingEvents.closing_date;
                    existingEvent.days_left_to_close = CommonHelper.DiffDays(dtoUpcomingEvents.closing_date);
                    existingEvent.event_type = dtoUpcomingEvents.event_type;
                    existingEvent.remark = dtoUpcomingEvents.remark;
                    existingEvent.relation_id = dtoUpcomingEvents.relation_id;

                    existingEvent.modify_id = UserContext.Current.UserId;
                    existingEvent.modify_name = UserContext.Current.UserName;
                    existingEvent.modify_date = DateTime.Now;

                    existingEvent.recipient_user_id = dtoUpcomingEvents.recipient_user_id;

                    var res = _repository.Update(existingEvent);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                });

                return result;
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_Edit Exception：{e.Message}", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }


        /// <summary>
        /// 修改即将到来的事件，调用的时候特别注意需要执行SaveChange()
        /// </summary>
        /// <param name="upcomingEventDto">事件数据传输对象</param>
        /// <returns>操作结果</returns>
        /// <returns>
        /// **********因未进行事务的提交，所以调用的时候特别注意需要执行SaveChange()**********
        /// 
        /// </returns>
        public async Task<WebResponseContent> EditAsync(Upcoming_EventsDto dtoUpcomingEvents)
        {
            try
            {
                // 参数验证
                if (dtoUpcomingEvents == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("event_null"));

                // 查询要修改的实体
                Biz_Upcoming_Events existingEvent = await _repository.FindAsyncFirst(p => p.id == dtoUpcomingEvents.id && p.delete_status == 0);

                if (existingEvent == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));

                existingEvent.event_no = dtoUpcomingEvents.event_no;
                existingEvent.event_name = dtoUpcomingEvents.event_name;
                existingEvent.event_name_eng = dtoUpcomingEvents.event_name_eng;
                existingEvent.closing_date = dtoUpcomingEvents.closing_date;
                existingEvent.days_left_to_close = CommonHelper.DiffDays(dtoUpcomingEvents.closing_date);
                existingEvent.event_type = dtoUpcomingEvents.event_type;
                existingEvent.remark = dtoUpcomingEvents.remark;
                existingEvent.relation_id = dtoUpcomingEvents.relation_id;

                existingEvent.modify_id = UserContext.Current.UserId;
                existingEvent.modify_name = UserContext.Current.UserName;
                existingEvent.modify_date = DateTime.Now;

                existingEvent.recipient_user_id = dtoUpcomingEvents.recipient_user_id;

                _repository.Update(existingEvent);

                return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));

            }
            catch (Exception e)
            {
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_Edit Exception：{e.Message}", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除即将到来的事件，调用的时候特别注意需要执行SaveChange()
        /// </summary>
        /// <param name="guidRelationId">关系Id</param>
        /// <param name="guidRelationId">事件类型</param>
        /// <returns>
        /// **********因未进行事务的提交，所以调用的时候特别注意需要执行SaveChange()**********
        /// </returns>
        public WebResponseContent Del(Guid guidRelationId, UpcomingEventsEnum eventEnum)
        {
            try
            {
                // 使用事务修改数据
                WebResponseContent result = _repository.DbContextBeginTransaction(() =>
                {
                    // 查询要修改的实体
                    Biz_Upcoming_Events existingEvent = _repository.Find(p => p.relation_id == guidRelationId && p.event_type == (int)eventEnum && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();

                    if (existingEvent == null)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));

                    existingEvent.delete_status = (int)SystemDataStatus.Invalid;
                    existingEvent.modify_id = UserContext.Current.UserId;
                    existingEvent.modify_name = UserContext.Current.UserName;
                    existingEvent.modify_date = DateTime.Now;

                    var res = _repository.Update(existingEvent);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("delete") + _localizationService.GetString("failes"));
                });

                return result;
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_Edit Exception：{e.Message}", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除即将到来的事件，调用的时候特别注意需要执行SaveChange()
        /// </summary>
        /// <param name="guidRelationId">关系Id</param>
        /// <param name="guidRelationId">事件类型</param>
        /// <returns>
        /// **********因未进行事务的提交，所以调用的时候特别注意需要执行SaveChange()**********
        /// </returns>
        public async Task<WebResponseContent> DelAsync(Guid guidRelationId, UpcomingEventsEnum eventEnum)
        {
            try
            {
                // 查询要修改的实体
                Biz_Upcoming_Events existingEvent = await _repository.FindAsyncFirst(p => p.relation_id == guidRelationId && p.event_type == (int)eventEnum && p.delete_status == (int)SystemDataStatus.Valid);

                if (existingEvent == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));

                existingEvent.delete_status = (int)SystemDataStatus.Invalid;
                existingEvent.modify_id = UserContext.Current.UserId;
                existingEvent.modify_name = UserContext.Current.UserName;
                existingEvent.modify_date = DateTime.Now;

                _repository.Update(existingEvent);

                return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_Edit Exception：{e.Message}", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取relation_id
        /// </summary>
        /// <param name="guidRelationId">关系Id</param>
        /// <param name="guidRelationId">事件类型</param>
        /// <returns></returns>
        public Guid GetRelationID(Guid guidRelationId, UpcomingEventsEnum eventEnum)
        {
            Guid res = Guid.Empty;
            try
            {
                var model = _repository.Find(p => p.relation_id == guidRelationId && p.event_type == (int)eventEnum && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                if (model != null)
                    res = model.id;
            }
            catch(Exception e)
            {
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_Edit Exception：{e.Message}", e);
            }
            return res;
        }

        /// <summary>
        /// 获取relation_id
        /// </summary>
        /// <param name="guidRelationId">关系Id</param>
        /// <param name="guidRelationId">事件类型</param>
        /// <returns></returns>
        public async Task<Guid> GetRelationIDAsync(Guid guidRelationId, UpcomingEventsEnum eventEnum)
        {
            Guid res = Guid.Empty;
            try
            {
                var model = await _repository.FindAsyncFirst(p => p.relation_id == guidRelationId && p.event_type == (int)eventEnum && p.delete_status == (int)SystemDataStatus.Valid);
                if (model != null)
                    res = model.id;
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(this.GetType(), $"{this.GetType().Name}_Edit Exception：{e.Message}", e);
            }
            return res;
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task FinishAsync(Guid id)
        {
            var model = await _repository.FindAsyncFirst(p => p.id == id);
            if (model != null)
            {
                model.delete_status = (int)SystemDataStatus.CompleteClose;
                model.modify_id = UserContext.Current.UserId;
                model.modify_name = UserContext.Current.UserName;
                model.modify_date = DateTime.Now;
                _repository.Update(model);
            }
        }
    }
}

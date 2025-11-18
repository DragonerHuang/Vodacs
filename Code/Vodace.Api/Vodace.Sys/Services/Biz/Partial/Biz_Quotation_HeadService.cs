
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
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
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_HeadService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Quotation_HeadRepository _repository;//访问数据库

        private readonly ISys_ContactRepository _contactRepository;                             // 联系人仓储
        private readonly IBiz_Contact_RelationshipRepository _contactRelationshipRepository;    // 联系人仓储

        private readonly ILocalizationService _localizationService;                             // 国际化
        private readonly IBiz_Quotation_DeadlineService _deadlineService;                       // 期限管理服务

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_HeadService(
            IBiz_Quotation_HeadRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            ISys_ContactRepository contactRepository,
            IBiz_Contact_RelationshipRepository contactRelationshipRepository,
            IBiz_Quotation_DeadlineService deadlineService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _contactRepository = contactRepository;
            _contactRelationshipRepository = contactRelationshipRepository;
            _deadlineService = deadlineService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 负责列表中人员下拉列表
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContactDownListAsync(Guid qnId)
        {
            try
            {
                var lstRelContact = await _contactRelationshipRepository
                    .FindAsync(p => p.relation_id == qnId && p.relation_type == 0 && p.delete_status == (int)SystemDataStatus.Valid);
                var lstContactIds = lstRelContact.Select(p => p.contact_id).Distinct();

                var lstContact = await _contactRepository
                    .FindAsIQueryable(p => lstContactIds.Contains(p.id))
                    .OrderByDescending(p => p.create_date)
                    .ToListAsync();

                var lstDown = lstContact
                    .Select(x => new { x.id, x.name_eng, x.name_cht }).ToList();


                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstDown);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取负责人列表
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetHeadListAsync(Guid qnId)
        {
            try
            {
                Expression<Func<Biz_Quotation_Head, Sys_Contact, QnHeadDto>> select = (qn_head, contact) => new QnHeadDto
                {
                    qn_id = qn_head.qn_id,
                    id = qn_head.id,
                    handler_type_eng = qn_head.handler_type,
                    handler_id = qn_head.handler_id,
                    handler_cht = contact.name_cht,
                    handler_eng = contact.name_eng,
                    create_date = qn_head.create_date,
                    create_id = qn_head.create_id,
                    create_name = qn_head.create_name,
                };
                select = select.BuildExtendSelectExpre();

                // 获取截止日期数据和联系人数据
                var headQury = _repository.FindAsIQueryable(p => p.qn_id == qnId && p.delete_status == (int)SystemDataStatus.Valid).AsExpandable().AsNoTracking();
                var contactData = _contactRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid).AsNoTracking();

                // 构建左联查询
                var query = from d in headQury
                            join c in contactData on d.handler_id equals c.id into dc
                            from contact in dc.DefaultIfEmpty()
                            select @select.Invoke(d, contact);

                var qnHeadData = await query.OrderBy(p => p.create_date).ToListAsync();

                // 处理列表中英文
                foreach (var item in qnHeadData)
                {
                    item.handler_type_cht = QnHeadTypeHelper.GetQnHeadTypeStr(item.handler_type_eng);
                }

                if (qnHeadData.Count == 0)
                {
                    return await AddHeaders(qnId);
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), qnHeadData);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 当报价创建时创建负责人列表（未实现savechange）
        /// </summary>
        /// <param name="qnId"></param>
        public void AddHeadersByAddQn(Guid qnId)
        {
            var lstHeadlers = DoQnHead(qnId);

            _repository.AddRange(lstHeadlers);
        }

        /// <summary>
        /// 编辑负责人
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditHeadAsync(QnHeadInputDto input)
        {
            try
            {
                var headData = await _repository.FindFirstAsync(p => p.id == input.id);
                if (headData == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_head_not_exist"));
                }
                var oldHandlerId = headData.handler_id;

                headData.handler_id = input.contact_id;
                headData.modify_id = UserContext.Current.UserInfo.User_Id;
                headData.modify_name = UserContext.Current.UserInfo.UserName;
                headData.modify_date = DateTime.Now;

                _repository.Update(headData);


                //if (oldHandlerId != input.contact_id)
                //{
                //    var changeResult = await _deadlineService.EditByHeadlerChangeAsync(headData.qn_id.Value, input.contact_id, oldHandlerId, headData.handler_type);
                //    if (!changeResult.status)
                //    {
                //        return changeResult;
                //    }
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
        /// 根据报价id和负责人类型获取对应的负责人
        /// </summary>
        /// <param name="qnId"></param>
        /// <param name="headlerType"></param>
        /// <returns></returns>
        public async Task<Guid?> GetHeadlerId(Guid qnId, string headlerType)
        {
            var headData = await _repository.FindAsIQueryable(p => p.qn_id == qnId && p.handler_type == headlerType && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefaultAsync();

            return headData?.handler_id;
        }


        /// <summary>
        /// 当报价没有负责人数据时，添加相对应的内容
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> AddHeaders(Guid qnId)
        {
            try
            {
                var lstHeadlers = DoQnHead(qnId);

                _repository.AddRange(lstHeadlers);
                await _repository.SaveChangesAsync();

                var lstDtoData = new List<QnHeadDto>();

                foreach (var item in lstHeadlers)
                {
                    lstDtoData.Add(new QnHeadDto
                    {
                        qn_id = item.qn_id,
                        id = item.id,
                        handler_type_eng = item.handler_type,
                        handler_type_cht = QnHeadTypeHelper.GetQnHeadTypeStr(item.handler_type),
                        handler_id = item.handler_id,
                        create_date = item.create_date,
                        create_id = item.create_id,
                        create_name = item.create_name,
                    });
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstDtoData); 
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 处理负责人数据
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        private List<Biz_Quotation_Head> DoQnHead(Guid qnId)
        {
            var lstHeadlers = new List<Biz_Quotation_Head>
            {
                new Biz_Quotation_Head {
                    id = Guid.NewGuid(),
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                    qn_id = qnId,
                    handler_type = "Tender Document",
                    remark = ""
                },
                new Biz_Quotation_Head {
                    id = Guid.NewGuid(),
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                    qn_id = qnId,
                    handler_type = "Sitevisit",
                    remark = ""
                },
                new Biz_Quotation_Head {
                    id = Guid.NewGuid(),
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                    qn_id = qnId,
                    handler_type = "Quotation",
                    remark = ""
                },
            };

            return lstHeadlers;
        }
       
    }


}

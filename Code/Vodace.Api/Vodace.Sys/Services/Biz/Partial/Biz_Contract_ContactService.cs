
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
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Biz_Contract_ContactService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Contract_ContactRepository _repository;//访问数据库

        private readonly ILocalizationService _localizationService;     // 国际化

        [ActivatorUtilitiesConstructor]
        public Biz_Contract_ContactService(
            IBiz_Contract_ContactRepository dbRepository,
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
        /// 保存合同联系人
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="lstContact"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveContactsAsync(Guid contractId, List<ContactQnDto> lstContact)
        {
            try
            {
                var lstIds = lstContact.Where(p => p.id.HasValue).Select(p => p.id).ToList();

                var lstNowRecords = await _repository.FindAsync(p => lstIds.Contains(p.id)); // 要修改的
                var dicNowRecords = lstNowRecords.ToDictionary(p => p.id);
                // 要删除的
                var lstBeforeRecords = await _repository.FindAsync(p => p.contract_id == contractId &&
                                                                        !lstIds.Contains(p.id) &&
                                                                        p.delete_status == (int)SystemDataStatus.Valid);

                var lstAddContact = new List<Biz_Contract_Contact>();
                var lstEditContact = new List<Biz_Contract_Contact>();

                // 新增或者修改
                foreach (var item in lstContact)
                {
                    if (!item.id.HasValue || item.id == Guid.Empty)
                    {
                        lstAddContact.Add(new Biz_Contract_Contact
                        {
                            id = Guid.NewGuid(),
                            contract_id = contractId,
                            contact_name = item.contact_name,
                            contact_tel = item.contact_phone,
                            contact_fax = item.contact_faxno,
                            contact_email = item.contact_mail,
                            contact_title = item.contact_tilte,
                            mail_to = item.contact_mail_to,
                            remark = "",

                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserInfo.User_Id,
                            create_name = UserContext.Current.UserInfo.UserName,
                            create_date = DateTime.Now,
                        });
                        continue;
                    }

                    var isDataOk = dicNowRecords.TryGetValue(item.id.Value, out var contactData);
                    if (!isDataOk) continue;

                    if (contactData.contact_name == item.contact_name &&
                        contactData.contact_tel == item.contact_phone &&
                        contactData.contact_fax == item.contact_faxno &&
                        contactData.contact_email == item.contact_mail &&
                        contactData.contact_title == item.contact_tilte &&
                        contactData.mail_to == item.contact_mail_to)
                    {
                        continue;
                    }

                    contactData.contact_name = item.contact_name;
                    contactData.contact_tel = item.contact_phone;
                    contactData.contact_fax = item.contact_faxno;
                    contactData.contact_email = item.contact_mail;
                    contactData.contact_title = item.contact_tilte;
                    contactData.mail_to = item.contact_mail_to;
                    contactData.modify_id = UserContext.Current.UserInfo.User_Id;
                    contactData.modify_name = UserContext.Current.UserInfo.UserName;
                    contactData.modify_date = DateTime.Now;

                    lstEditContact.Add(contactData);
                }

                // 删除
                foreach (var brforeItem in lstBeforeRecords)
                {
                    brforeItem.delete_status = (int)SystemDataStatus.Invalid;
                    brforeItem.modify_id = UserContext.Current.UserInfo.User_Id;
                    brforeItem.modify_name = UserContext.Current.UserInfo.UserName;
                    brforeItem.modify_date = DateTime.Now;
                    lstEditContact.Add(brforeItem);
                }

                _repository.AddRange(lstAddContact);
                _repository.UpdateRange(lstEditContact);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取合同的联系人
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public async Task<List<ContactQnDto>> GetContactsAsync(Guid contractId)
        {
            var contactData = await _repository
                    .FindAsIQueryable(p => p.contract_id == contractId && p.delete_status == (int)SystemDataStatus.Valid)
                    .OrderBy(p => p.create_date)
                    .ToListAsync();

            var lstDtoContact = new List<ContactQnDto>();
            foreach (var item in contactData)
            {
                lstDtoContact.Add(new ContactQnDto
                {
                    id = item.id,
                    contact_name = item.contact_name,
                    contact_tilte = item.contact_title,
                    contact_phone = item.contact_tel,
                    contact_mail = item.contact_email,
                    contact_faxno = item.contact_fax,
                    contact_mail_to = item.mail_to
                });
            }

            return lstDtoContact;
        }
    }
}

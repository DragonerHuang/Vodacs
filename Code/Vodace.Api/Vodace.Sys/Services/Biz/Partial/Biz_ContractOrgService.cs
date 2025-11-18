
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph.Models;
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

namespace Vodace.Sys.Services
{
    public partial class Biz_ContractOrgService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_ContractOrgRepository _repository;//访问数据库
        private readonly IBiz_ContractRepository _contractRepository;
        private readonly IBiz_QuotationRepository _qnRepository;
        private readonly IBiz_Contact_RelationshipRepository _relationRepository;
        private readonly ISys_OrganizationRepository _orgRepository;
        private readonly ISys_User_NewRepository _userRepository;
        private readonly ISys_ContactRepository _contactRepository;
        private readonly ILocalizationService _localizationService;

        [ActivatorUtilitiesConstructor]
        public Biz_ContractOrgService(
            IBiz_ContractOrgRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IBiz_ContractRepository contractRepository,
            ISys_OrganizationRepository orgRepository,
            ISys_User_NewRepository userRepository,
            ISys_ContactRepository contactRepository,
            IBiz_QuotationRepository qnRepository,
            IBiz_Contact_RelationshipRepository relationRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _contractRepository = contractRepository;
            _orgRepository = orgRepository;
            _userRepository = userRepository;
            _contactRepository = contactRepository;
            _qnRepository = qnRepository;
            _relationRepository = relationRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public WebResponseContent Add(ContractOrgDto dto)
        {
            try
            {
                Biz_ContractOrg model = new Biz_ContractOrg();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                var check = CheckEdit(dto);
                if (!string.IsNullOrEmpty(check))
                    return WebResponseContent.Instance.Error(check);

                model.master_id = dto.master_id;
                model.contract_id = dto.contract_id;
                model.org_id = dto.org_id;
                model.user_id = dto.user_id;
                model.is_special = dto.is_special;
                model.submit_file_code = dto.submit_file_code;
                model.remark = dto.remark;

                _repository.Add(model);

                _repository.SaveChanges();

                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractOrgService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent AddBatch(List<ContractOrgDto> lstDto)
        {
            try
            {
                if (lstDto.Count > 0)
                {
                    var time = DateTime.Now;
                    var userId = UserContext.Current.UserId;
                    var userName = UserContext.Current.UserName;

                    List<Biz_ContractOrg> list = new List<Biz_ContractOrg>();
                    foreach (var dto in lstDto)
                    {
                        var model = _repository.WhereIF(true, a => a.id == dto.id).FirstOrDefault();

                        if (!dto.id.HasValue || model == null)
                        {
                            model = new Biz_ContractOrg();
                            model.id = Guid.NewGuid();
                            model.delete_status = (int)SystemDataStatus.Valid;
                            model.create_id = userId;
                            model.create_name = userName;
                            model.create_date = time;

                            var check = CheckEdit(dto);
                            if (!string.IsNullOrEmpty(check))
                                return WebResponseContent.Instance.Error(check);

                            model.master_id = dto.master_id;
                            model.contract_id = dto.contract_id;
                            model.org_id = dto.org_id;
                            model.user_id = dto.user_id;
                            model.is_special = dto.is_special;
                            model.submit_file_code = dto.submit_file_code;
                            model.remark = dto.remark;

                            _repository.Add(model);
                        }
                        else
                        {
                            model.modify_id = userId;
                            model.modify_name = userName;
                            model.modify_date = time;

                            var check = CheckEdit(dto);
                            if (!string.IsNullOrEmpty(check))
                            {
                                //一致情况下，不修改这几个参数
                                //model.master_id = dto.master_id;
                                //model.contract_id = dto.contract_id;
                                //model.org_id = dto.org_id;
                                //model.user_id = dto.user_id;
                            }
                            else
                            {
                                model.master_id = dto.master_id;
                                model.contract_id = dto.contract_id;
                                model.org_id = dto.org_id;
                                model.user_id = dto.user_id;
                            }
                            model.is_special = dto.is_special;
                            model.submit_file_code = dto.submit_file_code;
                            model.remark = dto.remark;

                            _repository.Update(model);
                        }
                        list.Add(model);
                    }
                    _repository.SaveChanges();
                    return WebResponseContent.Instance.OK("Ok", list);
                }
                return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "lstDto"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractOrgService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                var model = repository.Find(p => p.id == guid).FirstOrDefault();
                if (model != null)
                {
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
                Log4NetHelper.Error("Biz_ContractOrgService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(ContractOrgDto dto)
        {
            try
            {
                var model = repository.Find(p => p.id == dto.id).FirstOrDefault();
                if (model != null)
                {
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    model.master_id = dto.master_id;
                    model.contract_id = dto.contract_id;
                    model.org_id = dto.org_id;
                    model.user_id = dto.user_id;
                    model.is_special = dto.is_special;
                    model.submit_file_code = dto.submit_file_code;
                    model.remark = dto.remark;

                    var check = CheckEdit(dto);
                    if (!string.IsNullOrEmpty(check))
                        return WebResponseContent.Instance.Error(check);

                    var res = _repository.Update(model);

                    _repository.SaveChanges();

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
                Log4NetHelper.Error("Biz_ContractOrgService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetContractOrgList(PageInput<ContractOrgSearchDto> dto)
        {
            try
            {
                var list = _repository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstContract = _contractRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstOrg = _orgRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstUser = _userRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstContact = _contactRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);

                var query = from r in list
                            join t in list
                            on r.master_id equals t.id into master
                            from lt in master.DefaultIfEmpty()

                            join c in lstContract
                            on r.contract_id equals c.id into contract
                            from rc in contract.DefaultIfEmpty()

                            join o in lstOrg
                            on r.org_id equals o.id into org
                            from ro in org.DefaultIfEmpty()

                            join u in lstUser
                            on r.user_id equals u.user_id into user
                            from ru in user.DefaultIfEmpty()

                            join uc in lstContact
                            on ru.contact_id equals uc.id into contact
                            from ucc in contact.DefaultIfEmpty()

                            select new ContractOrgListDto()
                            {
                                id = r.id,

                                master_id = r.master_id,
                                contract_id = r.contract_id,
                                is_special = r.is_special,
                                org_id = r.org_id,
                                user_id = r.user_id,
                                submit_file_code = r.submit_file_code,
                                remark = r.remark,

                                contract_name_cht = rc.name_cht,
                                contract_name_eng = rc.name_eng,
                                org_name_cht = ro.name_cht,
                                org_name_eng = ro.name_eng,
                                user_name = ru.user_name,
                                user_contact_name_cht = ucc.name_cht,
                                user_contact_name_eng = ucc.name_eng,

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
                    query = query.WhereIF(searchDto.master_id.HasValue, p => p.master_id == searchDto.master_id);
                    query = query.WhereIF(searchDto.id.HasValue, p => p.id == searchDto.id);
                    query = query.WhereIF(searchDto.contract_id.HasValue, p => p.contract_id == searchDto.contract_id);
                    query = query.WhereIF(searchDto.org_id.HasValue, p => p.org_id == searchDto.org_id);
                    query = query.WhereIF(searchDto.user_id > 0, p => p.user_id == searchDto.user_id);

                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.submit_file_code), p => p.submit_file_code.Contains(searchDto.submit_file_code));
                }

                if (!string.IsNullOrEmpty(dto.sort_field))
                    query = query.OrderByDynamic(dto.sort_field, dto.sort_type);
                else
                    query = query.OrderByDescending(x => x.create_date);

                var sql = query.ToQueryString();

                if (searchDto.is_tree == 1)
                {
                    //树节点结构（无分页）
                    var result = query.ToList();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), GetTree(result));
                }
                else
                {
                    // 执行分页查询（使用项目提供的扩展方法）
                    var result = await query.GetPageResultAsync(dto);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractOrgService.GetContractOrgList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #region  -- 私有 --

        private string CheckEdit(ContractOrgDto dto)
        {
            string res = string.Empty;
            try
            {
                #region  -- 唯一判断 --

                if (!dto.contract_id.HasValue)
                    return _localizationService.GetString("key_type_null", "contract_id");

                if (!dto.org_id.HasValue)
                    return _localizationService.GetString("key_type_null", "org_id");

                if (dto.user_id <= 0)
                    return _localizationService.GetString("key_type_null", "user_id");

                var exist = _repository.Find(a => a.contract_id == dto.contract_id && a.org_id == dto.org_id && a.user_id == dto.user_id && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                if (exist.Count > 0)
                {
                    return _localizationService.GetString("contract_org_exist");
                }
                var special = _repository.Find(a => a.contract_id == dto.contract_id && a.is_special == 1 && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                var lstOrg = _orgRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid).ToList();

                string name = string.Empty;
                if (lstOrg.Count > 0)
                {
                    var org = lstOrg.Where(a => a.id == dto.org_id).FirstOrDefault();
                    if (org != null)
                    {
                        if (UserContext.Current.Lang == (int)LangType.en_US)
                            name = org.name_eng;
                        else
                            name = org.name_cht;
                    }
                }

                if (dto.is_special == 1)
                {
                    if (!dto.master_id.HasValue)
                    {
                        return _localizationService.GetString("contract_org_masterid_special_exist", name);
                    }
                    //判断当前合约是否已存在一个is_special == 1
                    if (special != null)
                    {
                        return _localizationService.GetString("contract_org_special_exist", name);
                    }
                }
                //判断master_id是否等于is_special的id
                if (special != null && dto.master_id == special.id)
                {
                    return _localizationService.GetString("contract_org_masterid_special_null", name);
                }
                if (dto.master_id.HasValue)
                {
                    var master = _repository.Find(a => a.contract_id == dto.contract_id && a.id == dto.master_id && a.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                    if (master == null)
                    {
                        return _localizationService.GetString("contract_org_masterid_null");
                    }
                }

                #endregion

                #region  -- 报价联系人校验 --

                //如果联系人的职位与当前的组织不一致，同步报价联系人信息
                var quotation = _qnRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid && a.contract_id == dto.contract_id).FirstOrDefault();
                var user = _userRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid && a.user_id == dto.user_id).FirstOrDefault();
                
                if(quotation == null)
                    return _localizationService.GetString("key_type_null", (UserContext.Current.Lang == (int)LangType.en_US ? "Quotation" : "报价"));

                if (user == null)
                    return _localizationService.GetString("key_type_null", (UserContext.Current.Lang == (int)LangType.en_US ? "User" : "用户"));

                var relation = _relationRepository.Find(a => a.delete_status == (int)SystemDataStatus.Valid && a.relation_type == 0 && a.relation_id == quotation.id && a.contact_id == user.contact_id).First();
                if (relation != null)
                {
                    if (relation.org_id != dto.org_id)
                    {
                        //同步报价联系人信息
                        relation.org_id = dto.org_id;
                        relation.modify_date = DateTime.Now;
                        relation.modify_id = UserContext.Current.UserId;
                        _relationRepository.Update(relation);
                    }
                }
                else return _localizationService.GetString("key_type_null", (UserContext.Current.Lang == (int)LangType.en_US ? "Quotation contact person" : "报价联络人"));

                #endregion
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractOrgService.CheckEdit", e);
                return _localizationService.GetString("system_error");
            }
            return res;
        }

        private List<object> GetTree(List<ContractOrgListDto> list)
        {
            List<object> result = new List<object>();
            try
            {
                // 验证输入列表
                if (list == null || list.Count == 0)
                {
                    return result;
                }

                // 构建父子关系映射，提高查找效率
                var parentChildMap = new Dictionary<Guid, List<ContractOrgListDto>>();
                var allItems = new Dictionary<Guid, ContractOrgListDto>();

                // 初始化映射表
                foreach (var item in list)
                {
                    // 存储所有项目到字典中，用于快速查找
                    allItems[item.id] = item;

                    // 处理子节点映射
                    if (item.master_id.HasValue && item.master_id.Value != Guid.Empty)
                    {
                        if (!parentChildMap.ContainsKey(item.master_id.Value))
                        {
                            parentChildMap[item.master_id.Value] = new List<ContractOrgListDto>();
                        }
                        parentChildMap[item.master_id.Value].Add(item);
                    }
                }

                // 查找并处理所有根节点（没有父节点的节点）
                foreach (var item in list)
                {
                    // 根节点条件：master_id为null或empty或在列表中找不到对应的父节点
                    if (!item.master_id.HasValue || item.master_id.Value == Guid.Empty || !allItems.ContainsKey(item.master_id.Value))
                    {
                        // 递归构建节点及其子树
                        var treeNode = BuildTreeNode(item, parentChildMap, allItems);
                        result.Add(treeNode);
                    }
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractOrgService.GetTree", e);
            }
            return result;
        }

        /// <summary>
        /// 递归构建树节点
        /// </summary>
        /// <param name="item">当前节点数据</param>
        /// <param name="parentChildMap">父节点ID到子节点列表的映射</param>
        /// <param name="allItems">所有节点的字典</param>
        /// <returns>构建好的树节点对象</returns>
        private object BuildTreeNode(ContractOrgListDto item, Dictionary<Guid, List<ContractOrgListDto>> parentChildMap, Dictionary<Guid, ContractOrgListDto> allItems)
        {
            // 创建节点对象
            var node = new Dictionary<string, object>
            {
                { "id", item.id },
                { "master_id", item.master_id },
                { "contract_id", item.contract_id },
                { "is_special", item.is_special },
                { "org_id", item.org_id },
                { "user_id", item.user_id },
                { "submit_file_code", item.submit_file_code },
                { "remark", item.remark },

                { "contract_name_cht", item.contract_name_cht },
                { "contract_name_eng", item.contract_name_eng },
                { "org_name_cht", item.org_name_cht },
                { "org_name_eng", item.org_name_eng },
                { "user_name", item.user_name },
                { "user_contact_name_cht", item.user_contact_name_cht },
                { "user_contact_name_eng", item.user_contact_name_eng },

                { "create_id", item.create_id },
                { "create_name", item.create_name },
                { "create_date", item.create_date },
                { "modify_id", item.modify_id },
                { "modify_name", item.modify_name },
                { "modify_date", item.modify_date },

                // 添加其他需要的属性
                { "children", new List<object>() }
            };

            // 检查当前节点是否有子节点
            if (parentChildMap.ContainsKey(item.id))
            {
                var children = node["children"] as List<object>;

                // 递归处理所有子节点
                foreach (var childItem in parentChildMap[item.id])
                {
                    var childNode = BuildTreeNode(childItem, parentChildMap, allItems);
                    children.Add(childNode);
                }
            }

            return node;
        }

        #endregion
    }
}

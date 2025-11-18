
using AutoMapper;
using Castle.Core.Resource;
using Dm;
using Dm.filter;
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Common;
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
using Vodace.Sys.Repositories;

namespace Vodace.Sys.Services
{
    public partial class Biz_ContractService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_ContractRepository _repository;//访问数据库
        private readonly IBiz_ProjectRepository _repositoryProject;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IBiz_Site_RelationshipRepository _siteRelationshipRepository;
        private readonly IBiz_SiteRepository _biz_SiteRepository;
        private readonly IBiz_QuotationRepository _quotationRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_ContractService(
            IBiz_ContractRepository dbRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IBiz_Site_RelationshipRepository siteRelationshipRepository,
            ILocalizationService localizationService,
            IBiz_QuotationRepository quotationRepository,
            IBiz_SiteRepository biz_SiteRepository,
            IBiz_ProjectRepository repositoryProject)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            _repositoryProject = repositoryProject;
            _biz_SiteRepository = biz_SiteRepository;
            _quotationRepository = quotationRepository;
            _siteRelationshipRepository = siteRelationshipRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }


        /// <summary>
        /// 新增合约
        /// </summary>
        /// <param name="m_Contract"></param>
        /// <returns></returns>
        public WebResponseContent Add(ContractDto contractDto)
        {
            try
            {
                if (contractDto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                Biz_Contract biz_Contract = _mapper.Map<Biz_Contract>(contractDto);
                biz_Contract.id = Guid.NewGuid();
                biz_Contract.company_id = UserContext.Current.UserInfo.Company_Id;
                biz_Contract.delete_status = 0;
                biz_Contract.contract_no = DataSequence.GetSequence(contractDto.vo_wo_type, contractDto.contract_no);
                biz_Contract.create_id = UserContext.Current.UserId;
                biz_Contract.create_name = UserContext.Current.UserName;
                biz_Contract.create_date = DateTime.Now;
                _repository.Add(biz_Contract, true);
                return WebResponseContent.Instance.OK("Ok", biz_Contract);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除合约
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                Biz_Contract m_Contract = repository.Find(p => p.id == guid).FirstOrDefault();
                if (m_Contract != null)
                {
                    m_Contract.delete_status = 1;
                    m_Contract.modify_id = UserContext.Current.UserId;
                    m_Contract.modify_name = UserContext.Current.UserName;
                    m_Contract.modify_date = DateTime.Now;
                    var res = _repository.Update(m_Contract, true);

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
                Log4NetHelper.Error("Biz_ContractService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改合约
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Edit(ContractDto biz_ContractDto)
        {
            try
            {
                if (string.IsNullOrEmpty(biz_ContractDto.id.ToString()) || biz_ContractDto.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                Biz_Contract biz_Contract = repository.Find(p => p.id == biz_ContractDto.id).FirstOrDefault();
                if (biz_Contract != null)
                {
                    biz_Contract.po_id = biz_ContractDto.project_id;
                    //biz_Contract.pro_id = biz_ContractDto.pro_id;
                    //biz_Contract.contract_no = biz_ContractDto.Number; //默认这个不支持修改
                    biz_Contract.name_sho = biz_ContractDto.name_sho;
                    biz_Contract.name_eng = biz_ContractDto.name_eng;
                    biz_Contract.name_cht = biz_ContractDto.name_cht;
                    biz_Contract.name_ali = biz_ContractDto.name_ali;
                    //biz_Contract.delete_status = biz_ContractDto.enable; //默认这个不支持修改
                    biz_Contract.remark = biz_ContractDto.remark;
                    //biz_Contract.issue_date = biz_ContractDto.issue_date;
                    //biz_Contract.end_date = biz_ContractDto.end_date;
                    biz_Contract.title = biz_ContractDto.title;
                    biz_Contract.category = biz_ContractDto.category;
                    //biz_Contract.trade = biz_ContractDto.trade;
                    //biz_Contract.antic_pql_sub_close_date = biz_ContractDto.antic_pql_sub_close_date;
                    //biz_Contract.antic_pql_sub_date = biz_ContractDto.antic_pql_sub_date;
                    //biz_Contract.antic_inv_tndr_date = biz_ContractDto.antic_inv_tndr_date;
                    //biz_Contract.antic_cntr_awd_date = biz_ContractDto.antic_cntr_awd_date;
                    //biz_Contract.tender_type = biz_ContractDto.tender_type;
                    //biz_Contract.range_cost = biz_ContractDto.range_cost;
                    //biz_Contract.contact_name = biz_ContractDto.contact_name;
                    //biz_Contract.contact_title = biz_ContractDto.contact_title;
                    //biz_Contract.contact_email = biz_ContractDto.contact_email;
                    //biz_Contract.contact_tel = biz_ContractDto.contact_tel;
                    //biz_Contract.contact_fax = biz_ContractDto.contact_fax;
                    biz_Contract.ref_no = biz_ContractDto.ref_no;
                    //biz_Contract.qn_id = biz_ContractDto.qn_id;

                    biz_Contract.modify_id = UserContext.Current.UserId;
                    biz_Contract.modify_name = UserContext.Current.UserName;
                    biz_Contract.modify_date = DateTime.Now;
                    var res = _repository.Update(biz_Contract, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                }
                else return WebResponseContent.Instance.Error(biz_ContractDto.id + _localizationService.GetString("non_existent"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取合约列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContractList(PageInput<ContractSearchDto> searchDto)
        {
            //PageGridData<ContractListDto> pageGridData = new PageGridData<ContractListDto>();
            try
            {
                var qnSearch = searchDto.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = $" where x.delete_status={(int)SystemDataStatus.Valid} ";

                //if (UserContext.Current.UserInfo.Company_Id != null)
                //    strWhere += $" and x.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                //else
                //{
                //    if (!UserContext.Current.IsSuperAdmin)
                //    {
                //        strWhere += $" and x.company_id='{Guid.Empty}'";
                //    }
                //}

                //采用首页的获取公司数据的逻辑
                if (UserContext.Current.IsSuperAdmin)
                {
                    //查看所有公司数据
                }
                else if (UserContext.Current.UserInfo.Company_Id.HasValue)
                {
                    strWhere += $" and p.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                }

                if (!string.IsNullOrEmpty(qnSearch.vo_wo_type))
                {
                    strWhere += $" and x.vo_wo_type like '%{qnSearch.vo_wo_type}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.contract_name))
                {
                    strWhere += $" and (x.name_eng like '%{qnSearch.contract_name}%' or x.name_cht like '%{qnSearch.contract_name}%') ";
                }
                if (!string.IsNullOrEmpty(qnSearch.contract_no))
                {
                    strWhere += $" and x.contract_no like '%{qnSearch.contract_no}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.subject))
                {
                    strWhere += $" and x.subject like '%{qnSearch.subject}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.ref_no))
                {
                    strWhere += $" and x.ref_no like '%{qnSearch.ref_no}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.contract_address))
                {
                    strWhere += $" and x.contract_address like '%{qnSearch.contract_address}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.work_type_code))
                {
                    strWhere += $" and x.work_type_code like '%{qnSearch.work_type_code}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.title))
                {
                    strWhere += $" and x.title like '%{qnSearch.title}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.category))
                {
                    strWhere += $" and x.category like '%{qnSearch.category}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.trade))
                {
                    strWhere += $" and x.trade like '%{qnSearch.trade}%' ";
                }

                #endregion

                var sql = $@"select 
                       qn.qn_no as strQuotationNo,
                        x.id,
                        x.po_id,
                        x.project_id,
                        x.contract_no,
                        x.name_eng,
                        x.name_cht,
                        x.delete_status,
                        x.category,
                        x.tender_type,
                        x.ref_no,
                        x.vo_wo_type,
                        x.company_id,
                        x.create_date,
                        COALESCE(p.name_eng, p.name_cht) strProjectName,
                        p.exp_start_date,
                        p.act_start_date,
                        p.exp_end_date,
                        p.act_end_date,
                        e.site_names as strSiteName,
                        com.company_name strCompanyName
                from Biz_Contract x
                left join Biz_Project p on x.project_id=p.id
                left join Biz_Quotation qn on p.id=qn.contract_id
                left join Sys_Company com on x.company_id=com.id
                left join (
                SELECT 
                    sr.relation_id,
                    STUFF(
                        (
                            SELECT ',' + COALESCE(s.name_eng, s.name_cht)
                            FROM Biz_Site_Relationship sr_inner
                            LEFT JOIN Biz_Site s ON sr_inner.site_id = s.id
                            WHERE sr_inner.relation_id = sr.relation_id
                              AND sr_inner.relation_type = 0
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 
                        1, 1, '') AS site_names
                FROM Biz_Site_Relationship sr
                WHERE sr.relation_type = 0
                GROUP BY sr.relation_id
                ) e on x.id = e.relation_id
                {strWhere}";

                var list = DBServerProvider.SqlDapper.QueryQueryable<ContractListDto>(sql, null);
                //var result = await list.GetPageResultAsync(searchDto);
                var result = list.GetPageResult(searchDto);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
                //int skip = (searchDto.page_index - 1) * searchDto.page_rows;
                //pageGridData.data = list.Skip(skip).Take<ContractListDto>(searchDto.page_rows).ToList();
                //pageGridData.status = true;
                //pageGridData.total = list.Count;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_ContractService.GetContractList", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取合约ID及名称
        /// </summary>
        /// <returns></returns>
        /// <remarks>获取delete_status=0(未删除数据)</remarks>
        public async Task<WebResponseContent> GetContractAllName()
        {
            try
            {
                var list = _repository.Find(p => p.delete_status == (int)SystemDataStatus.Valid &&
                (UserContext.Current.UserInfo.Company_Id != null ? p.company_id == UserContext.Current.UserInfo.Company_Id : (!UserContext.Current.IsSuperAdmin ? p.company_id == Guid.Empty : false))
                )
                    .Select(x => new { x.id, x.name_eng, x.name_cht, x.contract_no }).ToList();
                return WebResponseContent.Instance.OK("OK", list);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ContractService.GetContractAllName", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据ID获取合约信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContractById(Guid guid)
        {
            ContractDetailDto res = new ContractDetailDto();
            try
            {
                var model = repository.FindAsIQueryable(x => x.id == guid).FirstOrDefault();
                if (model != null)
                {
                    var modelProject = _repositoryProject.FindAsIQueryable(x => x.id == model.project_id)
                        .Select(x => new
                        {
                            x.id,
                            x.name_eng,
                            x.name_cht,
                            x.project_no,
                            x.exp_start_date,
                            x.act_start_date,
                            x.exp_end_date,
                            x.act_end_date
                        }).FirstOrDefault();

                    //加载项目对应的合约及施工地点
                    var dbContext = DBServerProvider.DbContext;
                    var lstSiteRelationshipSql = from sr in dbContext.Set<Biz_Site_Relationship>()
                                                 where sr.relation_type == 0
                                                 join s in dbContext.Set<Biz_Site>() on sr.site_id equals s.id into sGroup
                                                 from s in sGroup.DefaultIfEmpty() // 左连接 Biz_Site
                                                 select new
                                                 {
                                                     sr.id,
                                                     sr.relation_id,
                                                     sr.relation_type,
                                                     sr.site_id,
                                                     s.name_eng,
                                                     s.name_cht
                                                 };
                    var lstSiteRelationship = lstSiteRelationshipSql.ToList();

                    res.id = model.id;
                    res.po_id = model.po_id;
                    res.project_id = model.project_id;
                    res.contract_no = model.contract_no;
                    res.name_eng = model.name_eng;
                    res.name_cht = model.name_cht;
                    res.delete_status = model.delete_status;
                    res.category = model.category;
                    res.tender_type = model.tender_type;
                    res.ref_no = model.ref_no;

                    res.create_id = model.create_id ?? 0;
                    res.create_name = model.create_name;
                    res.create_date = model.create_date;
                    res.modify_id = model.modify_id ?? 0;
                    res.modify_name = model.modify_name;
                    res.modify_date = model.modify_date;

                    res.master_id = model.master_id;
                    if (res.master_id != null)
                    {
                        var modelMaster = repository.FindAsIQueryable(x => x.id == res.master_id).FirstOrDefault();
                        if(modelMaster != null)
                        {
                            res.strMasterName = !string.IsNullOrEmpty(modelMaster.name_eng) ? modelMaster.name_eng : modelMaster.name_cht;
                            res.strMasterNo = modelMaster.contract_no;
                        }
                    }

                    res.company_id = model.company_id;
                    if (model.company_id != null)
                    {
                        var modelCompany = _repositoryProject.FindAsIQueryable(x => x.id == model.company_id).FirstOrDefault();
                        if (modelCompany != null)
                        {
                            res.strCompanyName = !string.IsNullOrEmpty(modelCompany.name_eng) ? modelCompany.name_eng : modelCompany.name_cht;
                        }
                    }

                    if (modelProject != null)
                    {
                        res.strProjectName = !string.IsNullOrEmpty(modelProject.name_eng) ? modelProject.name_eng : modelProject.name_cht;
                        res.exp_start_date = modelProject.exp_start_date;
                        res.act_start_date = modelProject.act_start_date;
                        res.exp_end_date = modelProject.exp_end_date;
                        res.act_end_date = modelProject.act_end_date;
                        res.project_no = modelProject.project_no;
                    }

                    res.strSiteName = string.Join(",", lstSiteRelationship.Where(x => x.relation_id == model.id).Select(x => x.name_eng));
                    if (!string.IsNullOrEmpty(res.strSiteName) && res.strSiteName.Length > 0)
                        res.strSiteName = res.strSiteName.substring(0, res.strSiteName.Length - 1);
                }
                return WebResponseContent.Instance.OK("Ok", res);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractService.GetContractById", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 设置合约的MasterID
        /// </summary>
        /// <param name="dtoSetContractMaster"></param>
        /// <returns></returns>
        public WebResponseContent SetContractMasterId(SetContractMasterDto dtoSetContractMaster)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoSetContractMaster.id.ToString()) || dtoSetContractMaster.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                if (string.IsNullOrEmpty(dtoSetContractMaster.vo_wo_type)) return WebResponseContent.Instance.Error($"vo_wo_type {_localizationService.GetString("connot_be_empty")}");

                if (dtoSetContractMaster.intSetType == 1)
                {
                    if (string.IsNullOrEmpty(dtoSetContractMaster.master_id.ToString()) || dtoSetContractMaster.master_id == Guid.Empty)
                        return WebResponseContent.Instance.Error($"master id {_localizationService.GetString("connot_be_empty")}");

                    if (dtoSetContractMaster.id == dtoSetContractMaster.master_id)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("contract_id_cannot_original_contract_id"));
                }

                //如果添加的时候，master_id为空，则通过报价单获取
                if (dtoSetContractMaster.intSetType == 1 && string.IsNullOrEmpty(dtoSetContractMaster.master_id.ToString()))
                {
                    if (!string.IsNullOrEmpty(dtoSetContractMaster.qn_no) || !string.IsNullOrEmpty(dtoSetContractMaster.qn_id.ToString()))
                    {
                        var qnInfo = _quotationRepository
                            .WhereIF(!string.IsNullOrEmpty(dtoSetContractMaster.qn_no), a => a.qn_no == dtoSetContractMaster.qn_no)
                            .WhereIF(!string.IsNullOrEmpty(dtoSetContractMaster.qn_id.ToString()) && dtoSetContractMaster.qn_id != Guid.Empty, a => a.id == dtoSetContractMaster.qn_id)
                            .FirstOrDefault();
                        if (qnInfo != null)
                        {
                            dtoSetContractMaster.master_id = qnInfo.contract_id;
                        }
                    }
                }

                //判断父级ID是否与当前ID存在关系
                //父子级合约不能循环设置，并且只能设置一级关系
                var bc = repository.Find(p => p.id == dtoSetContractMaster.master_id).FirstOrDefault();
                if (bc == null)
                    //return WebResponseContent.Instance.Error(dtoSetContractMaster.master_id + _localizationService.GetString("non_existent"));
                    return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent"));
                else
                {
                    if(!string.IsNullOrEmpty(bc.vo_wo_type))
                        return WebResponseContent.Instance.Error(_localizationService.GetString("contract_already_exists_vo_wo_type"));
                }

                Biz_Contract biz_Contract = repository.Find(p => p.id == dtoSetContractMaster.id).FirstOrDefault();
                if (biz_Contract != null)
                {
                    if (dtoSetContractMaster.intSetType == 1)
                    {
                        if (string.IsNullOrEmpty(biz_Contract.master_id.ToString()))
                        {
                            biz_Contract.master_id = dtoSetContractMaster.master_id;
                            biz_Contract.vo_wo_type = dtoSetContractMaster.vo_wo_type;
                        }
                        else return WebResponseContent.Instance.Error(biz_Contract.contract_no + _localizationService.GetString("contract_already_exists_vo_wo_type"));
                    }
                    else
                    {
                        //直接清空，不需要判断
                        biz_Contract.master_id = null;
                        biz_Contract.vo_wo_type = null;
                    }
                    biz_Contract.modify_id = UserContext.Current.UserId;
                    biz_Contract.modify_name = UserContext.Current.UserName;
                    biz_Contract.modify_date = DateTime.Now;
                    var res = _repository.Update(biz_Contract, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                }
                else return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractService.SetContractMasterId", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取子级合约列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetChildContractList(PageInput<ChildContractSearchDto> searchDto)
        {
            try
            {
                var qnSearch = searchDto.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = $" where c.delete_status={(int)SystemDataStatus.Valid} ";

                //if (UserContext.Current.UserInfo.Company_Id != null)
                //    strWhere += $" and c.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                //else
                //{
                //    if (!UserContext.Current.IsSuperAdmin)
                //    {
                //        strWhere += $" and c.company_id='{Guid.Empty}'";
                //    }
                //}

                //采用首页的获取公司数据的逻辑
                if (UserContext.Current.IsSuperAdmin)
                {
                    //查看所有公司数据
                }
                if (UserContext.Current.UserInfo.Company_Id.HasValue)
                {
                    strWhere += $" and c.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                }

                if (qnSearch.contract_id.HasValue)
                {
                    if(qnSearch.contract_include == 1)
                        strWhere += $" and c.id='{qnSearch.contract_id}' ";

                    strWhere += $" and c.master_id='{qnSearch.contract_id}' ";
                }

                if (qnSearch.quotation_id.HasValue)
                {
                    strWhere += $" and q.id='{qnSearch.quotation_id}' ";
                }

                if (qnSearch.project_id.HasValue)
                {
                    strWhere += $" and c.project_id='{qnSearch.project_id}' ";
                }


                if (!qnSearch.contract_id.HasValue && !qnSearch.quotation_id.HasValue && !qnSearch.project_id.HasValue)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_quotation_null"));
                }

                if (!string.IsNullOrEmpty(qnSearch.qn_no))
                {
                    strWhere += $" and q.qn_no='{qnSearch.qn_no}' ";
                }

                if (!string.IsNullOrEmpty(qnSearch.vo_wo_type))
                {
                    strWhere += $" and c.vo_wo_type like '%{qnSearch.vo_wo_type}%' ";
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("vo_wo_type_null"));
                }

                if (!string.IsNullOrEmpty(qnSearch.contract_name))
                {
                    strWhere += $" and (c.name_eng like '%{qnSearch.contract_name}%' or c.name_cht like '%{qnSearch.contract_name}%') ";
                }
                if (!string.IsNullOrEmpty(qnSearch.contract_no))
                {
                    strWhere += $" and c.contract_no like '%{qnSearch.contract_no}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.title))
                {
                    strWhere += $" and c.title like '%{qnSearch.title}%' ";
                }

                #endregion

                var sql = $@"select 
                             c.id contract_id, 
                             q.id quotation_id,
                             c.project_id,
                             q.qn_no, 
                             c.contract_no, 
                             c.vo_wo_type, 
                             c.name_cht, 
                             c.name_eng, 
                             q.confirm_amt, 
                            (select count(1) from biz_contractorg co where c.id=co.contract_id) contract_org,
                            (select top 1 submit_file_code from biz_contractorg co where c.id=co.contract_id and delete_status=0 order by create_date desc) submit_file_code,
                             q.qn_amt 
                            from Biz_Contract c
                            left join Biz_Quotation q on c.id=q.contract_id
                            {strWhere}
                            order by c.create_date desc
                            ";

                var list = DBServerProvider.SqlDapper.QueryQueryable<ChildContractDto>(sql, null);
                var result = list.GetPageResult(searchDto);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_ContractService.GetContractList", ex);
                return null;
            }
        }

        #region  -- 已弃用 --

        /// <summary>
        /// 获取合约列表
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetContractList_bak(PageDataOptions loadData)
        {
            List<ContractListDto> res = new List<ContractListDto>();
            try
            {
                var query = await repository.FindAsIQueryable(x => x.delete_status == 0).ToListAsync();
                var total = query.Count();

                //获取项目列表
                var lstContract = await repository.FindAsIQueryable(x => x.delete_status == 0)
                    .OrderByDescending(x => x.create_date)
                    .Skip((loadData.Page - 1) * loadData.Rows)
                    .Take(loadData.Rows)
                    .Select(x => new
                    {
                        x.id,
                        x.po_id,
                        x.project_id,
                        x.contract_no,
                        x.name_eng,
                        x.name_cht,
                        x.delete_status,
                        //x.issue_date,
                        //x.end_date,
                        //x.category,
                        //x.trade,
                        //x.antic_pql_sub_close_date,
                        //x.antic_pql_sub_date,
                        //x.antic_inv_tndr_date,
                        //x.antic_cntr_awd_date,
                        //x.tender_type,
                        //x.range_cost,
                        //x.contact_name,
                        //x.contact_title,
                        //x.contact_email,
                        //x.contact_tel,
                        //x.contact_fax,
                        x.ref_no,
                        x.create_date
                    })
                    .ToListAsync();
                if (lstContract.Count > 0)
                {
                    var lstProject = await _repositoryProject.FindAsIQueryable(x => x.delete_status == 0)
                        .Select(x => new
                        {
                            x.id,
                            x.name_eng,
                            x.name_cht,
                            x.exp_start_date,
                            x.act_start_date,
                            x.exp_end_date,
                            x.act_end_date
                        }).ToListAsync();

                    //加载项目对应的合约及施工地点
                    var dbContext = DBServerProvider.DbContext;
                    var lstSiteRelationshipSql = from sr in dbContext.Set<Biz_Site_Relationship>()
                                                 where sr.relation_type == 0
                                                 join s in dbContext.Set<Biz_Site>() on sr.site_id equals s.id into sGroup
                                                 from s in sGroup.DefaultIfEmpty() // 左连接 Biz_Site
                                                 select new
                                                 {
                                                     sr.id,
                                                     sr.relation_id,
                                                     sr.relation_type,
                                                     sr.site_id,
                                                     s.name_eng,
                                                     s.name_cht
                                                 };
                    var lstSiteRelationship = lstSiteRelationshipSql.ToList();

                    foreach (var item in lstContract)
                    {
                        ContractListDto model = new ContractListDto();
                        model.id = item.id;
                        model.po_id = item.po_id;
                        model.project_id = item.project_id;
                        model.contract_no = item.contract_no;
                        model.name_eng = item.name_eng;
                        model.name_cht = item.name_cht;
                        model.delete_status = item.delete_status;
                        //model.issue_date = item.issue_date;
                        //model.end_date = item.end_date;
                        //model.category = item.category;
                        //model.trade = item.trade;
                        //model.antic_pql_sub_close_date = item.antic_pql_sub_close_date;
                        //model.antic_pql_sub_date = item.antic_pql_sub_date;
                        //model.antic_inv_tndr_date = item.antic_inv_tndr_date;
                        //model.antic_cntr_awd_date = item.antic_cntr_awd_date;
                        //model.tender_type = item.tender_type;
                        //model.range_cost = item.range_cost;
                        //model.contact_name = item.contact_name;
                        //model.contact_title = item.contact_title;
                        //model.contact_email = item.contact_email;
                        //model.contact_tel = item.contact_tel;
                        //model.contact_fax = item.contact_fax;
                        model.ref_no = item.ref_no;
                        model.create_date = item.create_date;

                        var pcInfo = lstProject.Where(x => x.id == item.project_id).FirstOrDefault();
                        if (pcInfo != null)
                        {
                            model.strProjectName = !string.IsNullOrEmpty(pcInfo.name_eng) ? pcInfo.name_eng : pcInfo.name_cht;
                            model.exp_start_date = pcInfo.exp_start_date;
                            model.act_start_date = pcInfo.act_start_date;
                            model.exp_end_date = pcInfo.exp_end_date;
                            model.act_end_date = pcInfo.act_end_date;
                        }
                        model.strSiteName = string.Join(",", lstSiteRelationship.Where(x => x.relation_id == item.id).Select(x => x.name_eng));
                        if (!string.IsNullOrEmpty(model.strSiteName) && model.strSiteName.Length > 0)
                            model.strSiteName = model.strSiteName.substring(0, model.strSiteName.Length - 1);
                        res.add(model);
                    }
                }
                return WebResponseContent.Instance.OK("Ok", res);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_ContractService.GetContractList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public PageGridData<Biz_Contract> GetListByPage(PageDataOptions pageData)
        {
            QueryRelativeList = (List<SearchParameters> parameters) =>
            {
                QueryRelativeExpression = (IQueryable<Biz_Contract> queryable) =>
                {
                    string strContractNo = parameters.Where(x => x.Name == "contract_no").Select(s => s.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(strContractNo))
                    {
                        queryable = queryable.Where(x => x.contract_no == strContractNo);
                    }
                    string strContractName = parameters.Where(x => x.Name == "contract_name").Select(s => s.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(strContractName))
                    {
                        queryable = queryable.Where(x => x.name_sho == strContractName || x.name_cht == strContractName || x.name_eng == strContractName);
                    }
                    string strSite = parameters.Where(x => x.Name == "site").Select(s => s.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(strSite))
                    {
                        var subQuery = _biz_SiteRepository.Find(d => d.name_eng == strSite || d.name_cht == strSite || d.name_chs == strSite);
                        var siteRelation = _siteRelationshipRepository.Find(d => subQuery.Any(x => x.id == d.site_id));
                        queryable = queryable.Where(x => siteRelation.Any(d => d.relation_type == 1 && x.id == d.relation_id));
                    }
                    return queryable;
                };
            };
            return base.GetPageData(pageData);
        }

        public async Task<PageGridData<ContractListDto>> GetContractList(PageInput<QnSearchDto> searchDto)
        {
            PageGridData<ContractListDto> pageGridData = new PageGridData<ContractListDto>();
            try
            {
                var qnSearch = searchDto.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = " where 1=1 ";

                if (!string.IsNullOrEmpty(qnSearch.number))
                {
                    strWhere += $"AND qn.qn_no like '%{qnSearch.number}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.contract_no))
                {
                    strWhere += $"AND qn.qn_no like '%{qnSearch.contract_no}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.contract_name))
                {
                    strWhere += $"AND x.name_eng = '{qnSearch.contract_name}' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.site_names))
                {
                    strWhere += $"AND e.site_names in '({qnSearch.site_names})' ";
                }

                #endregion

                var sql = $@"select 
                       qn.qn_no as strQuotationNo,
                       x.id,
                       x.po_id,
                       x.project_id,
                       x.contract_no,
                       x.name_eng,
                       x.name_cht,
                       x.delete_status,
                       x.issue_date,
                       x.end_date,
                       x.category,
                       x.trade,
                       x.antic_pql_sub_close_date,
                       x.antic_pql_sub_date,
                       x.antic_inv_tndr_date,
                       x.antic_cntr_awd_date,
                       x.tender_type,
                       x.range_cost,
                       x.contact_name,
                       x.contact_title,
                       x.contact_email,
                       x.contact_tel,
                       x.contact_fax,
                       x.ref_no,
                       x.create_date,
                       COALESCE(p.name_eng, p.name_cht) strProjectName,
                       p.exp_start_date,
                       p.act_start_date,
                       p.exp_end_date,
                       p.act_end_date,
                       e.site_names as strSiteName
                from Biz_Contract x
                left join Biz_Project p on x.project_id=p.id
                left join Biz_Quotation qn on p.id=qn.contract_id
                left join (
                SELECT 
                    sr.relation_id,
                    STUFF(
                        (
                            SELECT ',' + COALESCE(s.name_eng, s.name_cht)
                            FROM Biz_Site_Relationship sr_inner
                            LEFT JOIN Biz_Site s ON sr_inner.site_id = s.id
                            WHERE sr_inner.relation_id = sr.relation_id
                              AND sr_inner.relation_type = 0
                            FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)'), 
                        1, 1, '') AS site_names
                FROM Biz_Site_Relationship sr
                WHERE sr.relation_type = 0
                GROUP BY sr.relation_id
                ) e on x.id = e.relation_id
                {strWhere}";

                var list = DBServerProvider.SqlDapper.QueryList<ContractListDto>(sql, null);
                int skip = (searchDto.page_index - 1) * searchDto.page_rows;
                pageGridData.data = list.Skip(skip).Take<ContractListDto>(searchDto.page_rows).ToList();
                pageGridData.status = true;
                pageGridData.total = list.Count;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_ContractService.GetContractList", ex);
            }
            return pageGridData;
        }

        //public async Task<PageGridData<ContractListDto>> GetContractList(PageInput<QnSearchDto> searchDto)
        //{
        //    PageGridData<ContractListDto> pageGridData = new PageGridData<ContractListDto>();
        //    try
        //    {
        //        var qnSearch = searchDto.search;
        //        string strWhere = string.Empty;

        //        #region 查询条件

        //        strWhere = " where 1=1 ";

        //        if (!string.IsNullOrEmpty(qnSearch.number))
        //        {
        //            strWhere += $"AND qn.qn_no like '%{qnSearch.number}%' ";
        //        }
        //        if (!string.IsNullOrEmpty(qnSearch.contract_no))
        //        {
        //            strWhere += $"AND qn.qn_no like '%{qnSearch.contract_no}%' ";
        //        }
        //        if (!string.IsNullOrEmpty(qnSearch.contract_name))
        //        {
        //            strWhere += $"AND x.name_eng = '{qnSearch.contract_name}' ";
        //        }
        //        if (!string.IsNullOrEmpty(qnSearch.site_names))
        //        {
        //            strWhere += $"AND e.site_names in '({qnSearch.site_names})' ";
        //        }

        //        #endregion

        //        var sql = $@"select 
        //               qn.qn_no as strQuotationNo,
        //               x.id,
        //               x.po_id,
        //               x.project_id,
        //               x.contract_no,
        //               x.name_eng,
        //               x.name_cht,
        //               x.delete_status,
        //               x.issue_date,
        //               x.end_date,
        //               x.category,
        //               x.trade,
        //               x.antic_pql_sub_close_date,
        //               x.antic_pql_sub_date,
        //               x.antic_inv_tndr_date,
        //               x.antic_cntr_awd_date,
        //               x.tender_type,
        //               x.range_cost,
        //               x.contact_name,
        //               x.contact_title,
        //               x.contact_email,
        //               x.contact_tel,
        //               x.contact_fax,
        //               x.ref_no,
        //               x.create_date,
        //               COALESCE(p.name_eng, p.name_cht) strProjectName,
        //               p.exp_start_date,
        //               p.act_start_date,
        //               p.exp_end_date,
        //               p.act_end_date,
        //               e.site_names as strSiteName
        //        from Biz_Contract x
        //        left join Biz_Project p on x.project_id=p.id
        //        left join Biz_Quotation qn on p.id=qn.contract_id
        //        left join (
        //        SELECT 
        //            sr.relation_id,
        //            STUFF(
        //                (
        //                    SELECT ',' + COALESCE(s.name_eng, s.name_cht)
        //                    FROM Biz_Site_Relationship sr_inner
        //                    LEFT JOIN Biz_Site s ON sr_inner.site_id = s.id
        //                    WHERE sr_inner.relation_id = sr.relation_id
        //                      AND sr_inner.relation_type = 0
        //                    FOR XML PATH(''), TYPE
        //                ).value('.', 'NVARCHAR(MAX)'), 
        //                1, 1, '') AS site_names
        //        FROM Biz_Site_Relationship sr
        //        WHERE sr.relation_type = 0
        //        GROUP BY sr.relation_id
        //        ) e on x.id = e.relation_id
        //        {strWhere}";

        //        var list = DBServerProvider.SqlDapper.QueryList<ContractListDto>(sql, null);
        //        int skip = (searchDto.page_index - 1) * searchDto.page_rows;
        //        pageGridData.data = list.Skip(skip).Take<ContractListDto>(searchDto.page_rows).ToList();
        //        pageGridData.status = true;
        //        pageGridData.total = list.Count;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4NetHelper.Error("Biz_ContractService.GetContractList", ex);
        //    }
        //    return pageGridData;
        //}
        #endregion
    }
}

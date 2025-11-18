
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Biz_ProjectService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_ProjectRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private DbContext dbContext = DBServerProvider.DbContext;
        private readonly IMapper _mapper;
        private readonly Biz_Upcoming_EventsService _eventUpcoming;


        [ActivatorUtilitiesConstructor]
        public Biz_ProjectService(
            IBiz_ProjectRepository dbRepository,
            ILocalizationService localizationService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            Biz_Upcoming_EventsService eventUpcoming
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService=localizationService;
            _eventUpcoming = eventUpcoming;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        WebResponseContent webResponse = new WebResponseContent();

        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Add(ProjectDto m_ProjectDto)
        {
            try
            {
                if (m_ProjectDto == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                //var list = (from a in dbContext.Set<Biz_Project>()
                //            join b in dbContext.Set<Sys_Company>()
                //            on a.company_id equals b.id
                //            where a.project_no == m_ProjectDto.project_no && a.company_id == m_ProjectDto.company_id
                //            orderby a.create_date
                //            select new
                //            {
                //                Com_Name = b.company_name,
                //                Number = a.project_no
                //            }).ToList();

                //if (list.Count > 0)
                //{
                //    return WebResponseContent.Instance.Error($"{list[0].Com_Name}{_localizationService.GetString("existent")}{m_ProjectDto.project_no}");
                //}

                Biz_Project m_Project = _mapper.Map<Biz_Project>(m_ProjectDto);

                m_Project.id = Guid.NewGuid();
                m_Project.company_id = UserContext.Current.UserInfo.Company_Id;
                m_Project.delete_status = (int)SystemDataStatus.Valid;
                m_Project.create_id = UserContext.Current.UserId;
                m_Project.create_name = UserContext.Current.UserName;
                m_Project.create_date = DateTime.Now;

                _repository.Add(m_Project);

                //_eventUpcoming.Add(new Upcoming_Events_OptionDto
                //{
                //    event_name = m_Project.name_eng,
                //    event_no = m_Project.project_no,
                //    closing_date = (DateTime)(m_Project.act_end_date != null ? m_Project.act_end_date : (m_Project.exp_end_date == null ? DateTime.Now : m_Project.exp_end_date)),
                //    event_type = (int)UpcomingEventsEnum.Project,
                //    relation_id = m_Project.id,
                //    remark = $"[{m_Project.project_no}]{m_Project.name_eng}过期时间{(DateTime)(m_Project.act_end_date != null ? m_Project.act_end_date : (m_Project.exp_end_date == null ? DateTime.Now : m_Project.exp_end_date))}"
                //});

                _repository.SaveChanges();

                return WebResponseContent.Instance.OK("Ok", m_Project);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                Biz_Project biz_Project = repository.Find(p => p.id == guid).FirstOrDefault();
                if (biz_Project != null)
                {
                    biz_Project.delete_status = 1;
                    biz_Project.modify_id = UserContext.Current.UserId;
                    biz_Project.modify_name = UserContext.Current.UserName;
                    biz_Project.modify_date = DateTime.Now;
                    _repository.Update(biz_Project);
                    _eventUpcoming.Del(guid, UpcomingEventsEnum.Project);
                    _repository.SaveChanges();

                    return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Edit(ProjectDto m_ProjectDto)
        {
            try
            {
                if (string.IsNullOrEmpty(m_ProjectDto.id.ToString()) || m_ProjectDto.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                Biz_Project biz_Project = repository.Find(p => p.id == m_ProjectDto.id).FirstOrDefault();
                if (biz_Project != null)
                {
                    //biz_Project.company_id = m_ProjectDto.company_id;
                    //biz_Project.company_id = UserContext.Current.UserInfo.Company_Id;
                    biz_Project.master_id = m_ProjectDto.master_id;
                    biz_Project.original_id = m_ProjectDto.original_id;
                    biz_Project.project_type_id = m_ProjectDto.pro_type_id;
                    biz_Project.customer_id = m_ProjectDto.customer_id;
                    //biz_Project.pro_no = m_ProjectDto.pro_no;                   //默认这个不支持修改
                    //biz_Project.delete_status = m_ProjectDto.delete_status;     //默认这个不支持修改
                    biz_Project.name_sho = m_ProjectDto.name_sho;
                    biz_Project.name_eng = m_ProjectDto.name_eng;
                    biz_Project.name_cht = m_ProjectDto.name_cht;
                    biz_Project.name_ali = m_ProjectDto.name_ali;
                    biz_Project.exp_start_date = m_ProjectDto.exp_start_date;
                    biz_Project.exp_end_date = m_ProjectDto.exp_end_date;
                    biz_Project.act_start_date = m_ProjectDto.act_start_date;
                    biz_Project.act_end_date = m_ProjectDto.act_end_date;
                    biz_Project.remark = m_ProjectDto.remark;

                    biz_Project.modify_id = UserContext.Current.UserId;
                    biz_Project.modify_name = UserContext.Current.UserName;
                    biz_Project.modify_date = DateTime.Now;
                    var res = _repository.Update(biz_Project);

                    //_eventUpcoming.Edit(new Upcoming_EventsDto {
                    //    id = _eventUpcoming.GetRelationID(biz_Project.id, UpcomingEventsEnum.Project),
                    //    event_name = biz_Project.name_eng,
                    //    event_no = biz_Project.project_no,
                    //    closing_date = (DateTime)(biz_Project.act_end_date != null ? biz_Project.act_end_date : (biz_Project.exp_end_date == null ? DateTime.Now : biz_Project.exp_end_date)),
                    //    event_type = (int)UpcomingEventsEnum.Project,
                    //    relation_id = biz_Project.id,
                    //    remark = $"[{biz_Project.project_no}]{biz_Project.name_eng}过期时间{(DateTime)(biz_Project.act_end_date != null ? biz_Project.act_end_date : (biz_Project.exp_end_date == null ? DateTime.Now : biz_Project.exp_end_date))}"
                    //});

                    _repository.SaveChanges();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"), biz_Project);
                }
                else return WebResponseContent.Instance.Error(m_ProjectDto.id + _localizationService.GetString("non_existent"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectList(PageInput<SearchProjectDto> dtoSearchInput)
        {
            PageGridData<ProjectListDto> pageGridData = new PageGridData<ProjectListDto>();
            try
            {
                var search = dtoSearchInput.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = " where p.delete_status = 0 ";


                //采用首页的获取公司数据的逻辑
                if (UserContext.Current.IsSuperAdmin)
                {
                    //查看所有公司数据
                }
                else if (UserContext.Current.UserInfo.Company_Id.HasValue)
                {
                    strWhere += $" and p.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                }

                //if (UserContext.Current.UserInfo.Company_Id != null)
                //    strWhere += $" and p.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                //else
                //{
                //    if (!UserContext.Current.IsSuperAdmin)
                //    {
                //        strWhere += $" and p.company_id='{Guid.Empty}'";
                //    }
                //}

                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.strProjectNo))
                    {
                        strWhere += $"AND p.project_no like '%{search.strProjectNo.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.strContractName))
                    {
                        strWhere += $"AND EXISTS(SELECT 1 FROM Biz_Contract c WHERE c.project_id = p.id AND c.name_eng like '%{search.strContractName.Replace("'", "''")}%' AND c.delete_status = 0) ";
                    }
                    if (!string.IsNullOrEmpty(search.strContractNo))
                    {
                        strWhere += $"AND EXISTS(SELECT 1 FROM Biz_Contract c WHERE c.project_id = p.id AND c.contract_no like '%{search.strContractNo.Replace("'", "''")}%' AND c.delete_status = 0) ";
                    }
                    if (!string.IsNullOrEmpty(search.strSiteName))
                    {
                        strWhere += $@"AND EXISTS(SELECT 1 FROM Biz_Contract c 
                            LEFT JOIN Biz_Site_Relationship sr ON c.id = sr.relation_id AND sr.relation_type = 0
                            LEFT JOIN Biz_Site s ON sr.site_id = s.id
                            WHERE c.project_id = p.id AND c.delete_status = 0
                            AND(s.name_eng like '%{search.strSiteName.Replace("'", "''")}%' OR s.name_cht like '%{search.strSiteName.Replace("'", "''")}%')) ";
                    }
                }

                #endregion

                // 构建包含多表连接的SQL查询，优化查询性能
                var sql = $@"SELECT 
                            p.id,
                            p.project_no,
                            p.name_eng,
                            p.name_cht,
                            p.exp_start_date,
                            p.act_start_date,
                            p.exp_end_date,
                            p.act_end_date,
                            p.create_date,
                            '0' as intContractProgress,
                            -- 合约名称字符串（使用FOR XML PATH和STUFF实现字符串拼接）
                            (SELECT STUFF(
                                (
                                    SELECT ',' + COALESCE(c.name_eng, c.name_cht)
                                    FROM Biz_Contract c
                                    WHERE c.project_id = p.id AND c.delete_status = 0
                                    FOR XML PATH(''), TYPE
                                ).value('.', 'NVARCHAR(MAX)'), 
                                1, 1, '')
                            ) AS strContractName,
                            -- 地点名称字符串（通过子查询连接获取施工地点）
                            (SELECT STUFF(
                                (
                                    SELECT ',' + COALESCE(s.name_eng, s.name_cht)
                                    FROM Biz_Contract c
                                    LEFT JOIN Biz_Site_Relationship sr ON c.id = sr.relation_id AND sr.relation_type = 0
                                    LEFT JOIN Biz_Site s ON sr.site_id = s.id
                                    WHERE c.project_id = p.id AND c.delete_status = 0 AND s.id IS NOT NULL
                                    GROUP BY s.id, COALESCE(s.name_eng, s.name_cht)
                                    FOR XML PATH(''), TYPE
                                ).value('.', 'NVARCHAR(MAX)'), 
                                1, 1, '')
                            ) AS strSiteName
                        FROM Biz_Project p
                        {strWhere}
                        ORDER BY p.create_date DESC";

                var list = DBServerProvider.SqlDapper.QueryQueryable<ProjectListDto>(sql, null);
                var result = list.GetPageResult(dtoSearchInput);

                // 处理返回的项目列表，确保数据完整性
                foreach (var item in result.data.ToList())
                {
                    // 获取该项目的合约和地点详细信息
                    var contractSql = $@"SELECT 
                                    c.id AS contract_id,
                                    c.contract_no,
                                    c.name_eng AS contract_name_eng,
                                    c.name_cht AS contract_name_cht,
                                    --c.issue_date,
                                    --c.end_date,
                                    s.id AS site_id,
                                    s.name_eng AS site_name_eng,
                                    s.name_cht AS site_name_cht
                                FROM Biz_Contract c
                                LEFT JOIN Biz_Site_Relationship sr ON c.id = sr.relation_id AND sr.relation_type = 0
                                LEFT JOIN Biz_Site s ON sr.site_id = s.id
                                WHERE c.project_id = '{item.id}' AND c.delete_status = 0 and s.id is not null";

                    var contractsData = DBServerProvider.SqlDapper.QueryList<dynamic>(contractSql, null);
                    var contractDict = new Dictionary<Guid, ProjectContractDto>();

                    // 构建合约和地点的关系
                    foreach (var contractItem in contractsData)
                    {
                        var contractId = contractItem.contract_id;
                        if (!contractDict.ContainsKey(contractId))
                        {
                            var contract = new ProjectContractDto
                            {
                                id = contractId,
                                contract_no = contractItem.contract_no,
                                name_eng = contractItem.contract_name_eng,
                                name_cht = contractItem.contract_name_cht,
                                //issue_date = contractItem.issue_date,
                                //end_date = contractItem.end_date,
                                sites = new List<ProjectContractSiteDto>()
                            };
                            contractDict[contractId] = contract;
                        }

                        var siteId = contractItem.site_id;
                        var siteDto = new ProjectContractSiteDto
                        {
                            id = siteId,
                            name_eng = contractItem.site_name_eng,
                            name_cht = contractItem.site_name_cht
                        };
                        contractDict[contractId].sites.Add(siteDto);
                    }

                    // 设置项目的合约列表
                    //item.children = contractDict.Values.ToList();                    
                    item.children = contractDict.Values.ToList();
                }


                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectList", e);
                return null;
            }
        }

        /// <summary>
        /// 获取项目合约列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectContractList(PageInput<ContractSearchDto> searchDto)
        {
            try
            {
                var qnSearch = searchDto.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = $" where p.delete_status={(int)SystemDataStatus.Valid} and co.delete_status={(int)SystemDataStatus.Valid} ";

                //if (UserContext.Current.UserInfo.Company_Id != null)
                //    strWhere += $" and p.company_id='{UserContext.Current.UserInfo.Company_Id}'";
                //else
                //{
                //    if (!UserContext.Current.IsSuperAdmin)
                //    {
                //        strWhere += $" and p.company_id='{Guid.Empty}'";
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
                if (!string.IsNullOrEmpty(qnSearch.qn_no))
                {
                    strWhere += $" and qn.qn_no like '%{qnSearch.qn_no}%' ";
                }
                if (!string.IsNullOrEmpty(qnSearch.strSiteName))
                {
                    strWhere += $" and e.site_names like '%{qnSearch.strSiteName}%' ";
                }

                #endregion

                //更改为只取已报价的数据
                var sql = $@"select 
                            qn.qn_no as strQuotationNo,
                            x.id,
                            x.name_eng,
                            x.name_cht,
                            x.po_id,
                            x.project_id,
                            x.contract_no,
                            x.delete_status,
                            x.category,
                            x.tender_type,
                            x.ref_no,
                            x.vo_wo_type,
                            x.company_id,
                            x.create_date,
                            (select count(1) from biz_contractorg co where x.id=co.contract_id) contract_org,
                            (select top 1 submit_file_code from biz_contractorg co where x.id=co.contract_id and delete_status=0 order by create_date desc) submit_file_code,
                            COALESCE(p.name_eng, p.name_cht) strProjectName,
                            p.project_no,
                            p.exp_start_date,
                            p.act_start_date,
                            p.exp_end_date,
                            p.act_end_date,
                            e.site_names as strSiteName,
                            com.company_name strCompanyName
                    from Biz_Confirmed_Order co 
                    inner join Biz_Project p on co.project_id=p.id
                    left join Biz_Contract x on p.id=x.project_id and co.contract_id = x.id
                    left join Biz_Quotation qn on x.id=qn.contract_id
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
                var result = list.GetPageResult(searchDto);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Biz_ProjectService.GetContractList", ex);
                return null;
            }
        }

        /// <summary>
        /// 获取项目ID及名称
        /// </summary>
        /// <returns></returns>
        /// <remarks>获取delete_status=0(未删除数据)</remarks>
        public async Task<WebResponseContent> GetProjectAllName()
        {
            try
            {
                var list = _repository.Find(p => p.delete_status == (int)SystemDataStatus.Valid && 
                (UserContext.Current.UserInfo.Company_Id != null ? p.company_id == UserContext.Current.UserInfo.Company_Id : (!UserContext.Current.IsSuperAdmin ? p.company_id == Guid.Empty : false))
                ).Select(x => new { x.id, x.name_eng, x.name_cht, x.project_no }).ToList();
                return WebResponseContent.Instance.OK("OK", list);
            }
            catch(Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectById", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据ID获取项目信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectById(Guid guid)
        {
            try
            {
                ProjectDetailDto dtoProjectDetail = new ProjectDetailDto();
                var detail = _repository.Find(p => p.id == guid).FirstOrDefault();
                if(detail != null)
                {
                    dtoProjectDetail.company_id = detail.company_id;
                    dtoProjectDetail.master_id = detail.master_id;
                    dtoProjectDetail.original_id = detail.original_id;
                    dtoProjectDetail.customer_id = detail.customer_id;
                    dtoProjectDetail.project_no = detail.project_no;
                    dtoProjectDetail.name_sho = detail.name_sho;
                    dtoProjectDetail.name_eng = detail.name_eng;
                    dtoProjectDetail.name_cht = detail.name_cht;
                    dtoProjectDetail.name_ali = detail.name_ali;
                    dtoProjectDetail.exp_start_date = detail.exp_start_date;
                    dtoProjectDetail.act_start_date = detail.act_start_date;
                    dtoProjectDetail.exp_end_date = detail.exp_end_date;
                    dtoProjectDetail.act_end_date = detail.act_end_date;
                    dtoProjectDetail.delete_status = detail.delete_status ?? 0;
                    dtoProjectDetail.remark = detail.remark;
                    dtoProjectDetail.create_id = detail.create_id ?? 0;
                    dtoProjectDetail.create_name = detail.create_name;
                    dtoProjectDetail.create_date = detail.create_date;
                    dtoProjectDetail.modify_id = detail.modify_id ?? 0;
                    dtoProjectDetail.modify_name = detail.modify_name;
                    dtoProjectDetail.modify_date = detail.modify_date;

                    var dbContext = DBServerProvider.DbContext;
                    var modelProject = _repository.Find(p => p.id == dtoProjectDetail.master_id).FirstOrDefault();
                    if (modelProject != null)
                        dtoProjectDetail.strMasterProjectName = !string.IsNullOrEmpty(modelProject.name_eng) ? modelProject.name_eng : modelProject.name_cht;

                    var modelCompany = dbContext.Set<Sys_Company>().Where(p => p.id == dtoProjectDetail.company_id).FirstOrDefault();
                    if (modelCompany != null)
                        dtoProjectDetail.strCompanyName = modelCompany.company_name;

                    var modelCustomer = dbContext.Set<Biz_Sub_Contractors>().Where(p => p.id == dtoProjectDetail.customer_id).FirstOrDefault();
                    if (modelCustomer != null)
                        dtoProjectDetail.strCustomerName = !string.IsNullOrEmpty(modelCustomer.name_eng) ? modelCustomer.name_eng : modelCustomer.name_cht;
                }

                return WebResponseContent.Instance.OK("OK", dtoProjectDetail);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectById", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取项目统计信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectStatistics()
        {
            try
            {
                var dbContext = DBServerProvider.DbContext;


                // 计算项目统计信息
                int intTotalProjects = 0;
                // 进行中项目
                int intOngoingProjects = 0;
                // 已完成项目
                int intCompletedProjects = 0;
                // 总收入统计（需要纳入会计系统，目前未实现，总收入设置为0）
                double doubleGeneralIncome = 0;

                if (UserContext.Current.IsSuperAdmin)
                {
                    intTotalProjects = await dbContext.Set<Biz_Project>().CountAsync(p => p.delete_status == 0);
                    intOngoingProjects = await dbContext.Set<Biz_Project>().CountAsync(p => p.delete_status == 0 && (p.act_end_date == null || p.act_end_date > DateTime.Now));
                    intCompletedProjects = await dbContext.Set<Biz_Project>().CountAsync(p => p.delete_status == 0 && p.act_end_date != null && p.act_end_date <= DateTime.Now);
                }
                else
                {
                    if (UserContext.Current.UserInfo.Company_Id.HasValue)
                    {
                        intTotalProjects = await dbContext.Set<Biz_Project>().CountAsync(p => p.delete_status == 0 && p.company_id == UserContext.Current.UserInfo.Company_Id);
                        intOngoingProjects = await dbContext.Set<Biz_Project>().CountAsync(p => p.delete_status == 0 && p.company_id == UserContext.Current.UserInfo.Company_Id && (p.act_end_date == null || p.act_end_date > DateTime.Now));
                        intCompletedProjects = await dbContext.Set<Biz_Project>().CountAsync(p => p.delete_status == 0 && p.company_id == UserContext.Current.UserInfo.Company_Id && p.act_end_date != null && p.act_end_date <= DateTime.Now);
                    }
                }
                var statistics = new
                {
                    intTotalProjects,
                    intOngoingProjects,
                    intCompletedProjects,
                    doubleGeneralIncome
                };
                return WebResponseContent.Instance.OK("Ok", statistics);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectStatistics", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectDownListAsync(string proName = "")
        {
            try
            {
                var proSreach = _repository
                    .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == UserContext.Current.UserInfo.Company_Id)
                    .Where(p => p.delete_status == (int)SystemDataStatus.Valid);

                if (!string.IsNullOrEmpty(proName))
                {
                    proSreach.Where(p => p.name_eng.Contains(proName));
                }

                var lstProjects = await proSreach.ToListAsync();

                //var lstProjects = await _repository
                //   .WhereIF(!UserContext.Current.IsSuperAdmin, p => p.company_id == UserContext.Current.UserInfo.Company_Id)
                //   .Where(p => p.delete_status == (int)SystemDataStatus.Valid)
                //   .ToListAsync();

                var lstProDowns = new List<ProjectDownDto>();
                foreach (var item in lstProjects)
                {
                    lstProDowns.Add(new ProjectDownDto
                    {
                        project_id = item.id,
                        project_no = item.project_no,
                        name_eng = item.name_eng,
                        name_ali = item.name_ali,
                        name_cht = item.name_cht
                    });
                }
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), lstProDowns);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #region  -- 已弃用 --

        /// <summary>
        /// 已弃用
        /// 获取项目列表
        /// </summary>
        /// <param name="dtoQnSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectList_bak(PageInput<QnSearchDto> dtoQnSearchInput)
        {
            List<ProjectListDto> res = new List<ProjectListDto>();
            try
            {
                // 使用T-SQL查询替代LINQ查询
                var dbContext = DBServerProvider.DbContext;

                // 计算分页参数
                int page = dtoQnSearchInput.page_index > 0 ? dtoQnSearchInput.page_index : 1;
                int pageSize = dtoQnSearchInput.page_rows > 0 ? dtoQnSearchInput.page_rows : 20;
                int skip = (page - 1) * pageSize;
                int take = pageSize;

                // 执行T-SQL查询获取项目列表
                var projectSql = $@"SELECT 
            p.id, 
            p.project_no, 
            p.name_eng, 
            p.name_cht, 
            p.exp_start_date, 
            p.act_start_date, 
            p.exp_end_date, 
            p.act_end_date, 
            p.create_date
        FROM Biz_Project p
        WHERE p.delete_status = 0
        ORDER BY p.create_date DESC
        OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

                // 使用FromSqlRaw执行SQL查询并获取项目列表
                var projects = await dbContext.Set<Biz_Project>().FromSqlRaw(projectSql)
                    .Select(p => new
                    {
                        p.id,
                        p.project_no,
                        p.name_eng,
                        p.name_cht,
                        p.exp_start_date,
                        p.act_start_date,
                        p.exp_end_date,
                        p.act_end_date,
                        p.create_date
                    })
                    .ToListAsync();

                if (projects.Count > 0)
                {
                    // 获取所有项目ID用于后续查询
                    var projectIds = projects.Select(p => p.id).ToList();

                    // 查询项目相关的合约和地点信息
                    var contractSiteSql = $@"SELECT 
                c.id AS contract_id,
                c.project_id,
                c.contract_no,
                c.issue_date,
                c.end_date,
                c.name_eng AS contract_name_eng,
                c.name_cht AS contract_name_cht,
                s.id AS site_id,
                s.name_eng AS site_name_eng,
                s.name_cht AS site_name_cht
            FROM Biz_Contract c
            LEFT JOIN Biz_Site_Relationship sr ON c.id = sr.relation_id AND sr.relation_type = 0
            LEFT JOIN Biz_Site s ON sr.site_id = s.id
            WHERE c.project_id IN ({string.Join(",", projectIds.Select(id => $"'{id}'"))})";

                    // 使用ADO.NET执行SQL查询获取合约和地点信息
                    using (var connection = dbContext.Database.GetDbConnection())
                    {
                        await connection.OpenAsync();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = contractSiteSql;
                            //command.CommandType = System.Data.CommandType.Text;

                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                // 构建项目-合约-地点的关系字典
                                var projectContractDict = new Dictionary<Guid, Dictionary<Guid, ProjectContractDto>>();
                                var projectInfoDict = new Dictionary<Guid, dynamic>();

                                // 先保存项目基本信息
                                foreach (var project in projects)
                                {
                                    projectInfoDict[project.id] = project;
                                    projectContractDict[project.id] = new Dictionary<Guid, ProjectContractDto>();
                                }

                                // 处理合约和地点信息
                                while (await reader.ReadAsync())
                                {
                                    var projectId = reader.GetGuid(reader.GetOrdinal("project_id"));
                                    var contractId = reader.GetGuid(reader.GetOrdinal("contract_id"));

                                    // 确保项目存在
                                    if (!projectContractDict.ContainsKey(projectId))
                                        continue;

                                    // 如果合约不存在，则创建新合约
                                    if (!projectContractDict[projectId].ContainsKey(contractId))
                                    {
                                        var contractNameEng = reader.IsDBNull(reader.GetOrdinal("contract_name_eng")) ? "" : reader.GetString(reader.GetOrdinal("contract_name_eng"));
                                        var contractNameCht = reader.IsDBNull(reader.GetOrdinal("contract_name_cht")) ? "" : reader.GetString(reader.GetOrdinal("contract_name_cht"));

                                        var contract = new ProjectContractDto
                                        {
                                            id = contractId,
                                            contract_no = reader.GetString(reader.GetOrdinal("contract_no")),
                                            name_eng = contractNameEng,
                                            name_cht = contractNameCht,
                                            //issue_date = reader.GetDateTime(reader.GetOrdinal("issue_date")),
                                            //end_date = reader.GetDateTime(reader.GetOrdinal("end_date")),
                                            sites = new List<ProjectContractSiteDto>()
                                        };

                                        projectContractDict[projectId][contractId] = contract;
                                    }

                                    // 处理地点信息
                                    if (!reader.IsDBNull(reader.GetOrdinal("site_id")))
                                    {
                                        var siteId = reader.GetGuid(reader.GetOrdinal("site_id"));
                                        var siteNameEng = reader.IsDBNull(reader.GetOrdinal("site_name_eng")) ? "" : reader.GetString(reader.GetOrdinal("site_name_eng"));
                                        var siteNameCht = reader.IsDBNull(reader.GetOrdinal("site_name_cht")) ? "" : reader.GetString(reader.GetOrdinal("site_name_cht"));

                                        // 检查地点是否已存在
                                        var contract = projectContractDict[projectId][contractId];
                                        if (!contract.sites.Any(s => s.id == siteId))
                                        {
                                            contract.sites.Add(new ProjectContractSiteDto
                                            {
                                                id = siteId,
                                                name_eng = siteNameEng,
                                                name_cht = siteNameCht
                                            });
                                        }
                                    }
                                }

                                // 构建最终结果
                                foreach (var project in projects)
                                {
                                    var model = new ProjectListDto
                                    {
                                        id = project.id,
                                        project_no = project.project_no,
                                        name_eng = project.name_eng,
                                        name_cht = project.name_cht,
                                        exp_start_date = project.exp_start_date,
                                        exp_end_date = project.exp_end_date,
                                        children = new List<ProjectContractDto>(),
                                        strContractName = "",
                                        strSiteName = ""
                                    };

                                    // 添加合约信息
                                    if (projectContractDict.ContainsKey(project.id))
                                    {
                                        foreach (var contract in projectContractDict[project.id].Values)
                                        {
                                            model.children.Add(contract);

                                            // 构建合约名称字符串
                                            var contractName = string.IsNullOrEmpty(contract.name_eng) ? contract.name_cht : contract.name_eng;
                                            if (!string.IsNullOrEmpty(contractName))
                                            {
                                                if (!string.IsNullOrEmpty(model.strContractName))
                                                    model.strContractName += ",";
                                                model.strContractName += contractName;
                                            }

                                            // 构建地点名称字符串
                                            foreach (var site in contract.sites)
                                            {
                                                var siteName = string.IsNullOrEmpty(site.name_eng) ? site.name_cht : site.name_eng;
                                                if (!string.IsNullOrEmpty(siteName) && !model.strSiteName.Contains(siteName))
                                                {
                                                    if (!string.IsNullOrEmpty(model.strSiteName))
                                                        model.strSiteName += ",";
                                                    model.strSiteName += siteName;
                                                }
                                            }
                                        }
                                    }

                                    res.Add(model);
                                }
                            }
                        }
                    }
                }

                return WebResponseContent.Instance.OK("Ok", res);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        private async Task<WebResponseContent> GetProjectList_bak(PageDataOptions loadData)
        {
            List<ProjectListDto> res = new List<ProjectListDto>();
            try
            {
                var query = await repository.FindAsIQueryable(x => x.delete_status == 0).ToListAsync();
                var total = query.Count();

                //获取项目列表
                var list = await repository.FindAsIQueryable(x => x.delete_status == 0)
                    .OrderByDescending(x => x.create_date)
                    .Skip((loadData.Page - 1) * loadData.Rows)
                    .Take(loadData.Rows)
                    .Select(x => new
                    {
                        x.id,
                        x.project_no,
                        x.name_eng,
                        x.name_cht,
                        x.exp_start_date,
                        x.act_start_date,
                        x.exp_end_date,
                        x.act_end_date,
                        x.create_date
                    })
                    .ToListAsync();

                if (list.Count() > 0)
                {
                    //加载项目对应的合约及施工地点
                    var dbContext = DBServerProvider.DbContext;
                    var pcListSql = from con in dbContext.Set<Biz_Contract>()
                                    join sr in dbContext.Set<Biz_Site_Relationship>() on con.id equals sr.site_id into srGroup
                                    from sr in srGroup.DefaultIfEmpty() // 左连接 M_Site_Relationship
                                    join s in dbContext.Set<Biz_Site>() on sr.site_id equals s.id into sGroup
                                    from s in sGroup.DefaultIfEmpty() // 左连接 M_Site
                                    select new
                                    {
                                        con.id,
                                        con.project_id,
                                        con.contract_no,
                                        //con.issue_date,
                                        //con.end_date,
                                        con.name_eng,
                                        con.name_cht,
                                        sr.site_id,
                                        Site_Name_Eng = s.name_eng,
                                        Site_Name_Cht = s.name_cht
                                    };
                    var pcList = pcListSql.ToList();

                    if (pcList.Count() > 0)
                    {
                        ProjectListDto model = new ProjectListDto();
                        List<ProjectContractDto> contractList = new List<ProjectContractDto>();
                        List<ProjectContractSiteDto> Sites = new List<ProjectContractSiteDto>();
                        //循环项目列表
                        foreach (var item in list)
                        {
                            model = new ProjectListDto();
                            var pcItem = pcList.Where(x => x.project_id == item.id).ToList();
                            if (pcItem != null)
                            {
                                //循环项目对应的合约
                                foreach (var pc in pcList)
                                {
                                    ProjectContractDto m_ProjectContractDto = new ProjectContractDto();
                                    if (pc.project_id == item.id)
                                    {
                                        model.strContractName += string.IsNullOrEmpty(pc.name_eng) ? pc.name_eng : pc.name_cht + ",";
                                        m_ProjectContractDto.id = pc.id;
                                        m_ProjectContractDto.contract_no = pc.contract_no;
                                        m_ProjectContractDto.name_eng = pc.name_eng;
                                        m_ProjectContractDto.name_cht = pc.name_cht;
                                        //m_ProjectContractDto.issue_date = pc.issue_date;
                                        //m_ProjectContractDto.end_date = pc.end_date;
                                        var pcSite = pcList.Where(x => x.project_id == item.id && x.site_id == pc.site_id).ToList();

                                        //循环合约对应的施工地点
                                        foreach (var site in pcSite)
                                        {
                                            model.strSiteName += string.IsNullOrEmpty(site.Site_Name_Cht) ? site.Site_Name_Eng : site.Site_Name_Cht + ",";
                                            ProjectContractSiteDto m_ProjectContractSiteDto = new ProjectContractSiteDto();
                                            m_ProjectContractSiteDto.id = site.id;
                                            m_ProjectContractSiteDto.name_eng = site.Site_Name_Eng;
                                            m_ProjectContractSiteDto.name_cht = site.Site_Name_Cht;
                                            Sites.Add(m_ProjectContractSiteDto);
                                        }
                                        m_ProjectContractDto.sites = Sites;
                                        contractList.Add(m_ProjectContractDto);
                                    }
                                }
                                model.children = contractList;
                            }
                            model.id = item.id;
                            model.project_no = item.project_no;
                            model.name_eng = item.name_eng;
                            model.name_cht = item.name_cht;
                            model.exp_start_date = item.exp_start_date;
                            model.exp_end_date = item.exp_end_date;

                            if (!string.IsNullOrEmpty(model.strContractName) && model.strContractName.Length > 0)
                                model.strContractName = model.strContractName.Substring(0, model.strContractName.Length - 1);

                            if (!string.IsNullOrEmpty(model.strSiteName) && model.strSiteName.Length > 0)
                                model.strSiteName = model.strSiteName.Substring(0, model.strSiteName.Length - 1);

                            res.Add(model);
                        }
                    }
                }

                return WebResponseContent.Instance.OK("Ok", res);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        public async Task<PageGridData<ProjectListDto>> GetProjectList_bak3(PageInput<SearchProjectDto> dtoSearchInput)
        {
            PageGridData<ProjectListDto> pageGridData = new PageGridData<ProjectListDto>();
            try
            {
                var search = dtoSearchInput.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = " where 1=1 and p.delete_status = 0 ";

                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.strProjectNo))
                    {
                        strWhere += $"AND p.project_no like '%{search.strProjectNo.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.strContractName))
                    {
                        strWhere += $"AND EXISTS(SELECT 1 FROM Biz_Contract c WHERE c.project_id = p.id AND c.name_eng like '%{search.strContractName.Replace("'", "''")}%' AND c.delete_status = 0) ";
                    }
                    if (!string.IsNullOrEmpty(search.strContractNo))
                    {
                        strWhere += $"AND EXISTS(SELECT 1 FROM Biz_Contract c WHERE c.project_id = p.id AND c.contract_no like '%{search.strContractNo.Replace("'", "''")}%' AND c.delete_status = 0) ";
                    }
                    if (!string.IsNullOrEmpty(search.strSiteName))
                    {
                        strWhere += $@"AND EXISTS(SELECT 1 FROM Biz_Contract c 
                            LEFT JOIN Biz_Site_Relationship sr ON c.id = sr.relation_id AND sr.relation_type = 0
                            LEFT JOIN Biz_Site s ON sr.site_id = s.id
                            WHERE c.project_id = p.id AND c.delete_status = 0
                            AND(s.name_eng like '%{search.strSiteName.Replace("'", "''")}%' OR s.name_cht like '%{search.strSiteName.Replace("'", "''")}%')) ";
                    }
                }

                #endregion

                // 构建包含多表连接的SQL查询，优化查询性能
                var sql = $@"SELECT 
                            p.id,
                            p.project_no,
                            p.name_eng,
                            p.name_cht,
                            p.exp_start_date,
                            p.act_start_date,
                            p.exp_end_date,
                            p.act_end_date,
                            p.create_date,
                            -- 合约名称字符串（使用FOR XML PATH和STUFF实现字符串拼接）
                            (SELECT STUFF(
                                (
                                    SELECT ',' + COALESCE(c.name_eng, c.name_cht)
                                    FROM Biz_Contract c
                                    WHERE c.project_id = p.id AND c.delete_status = 0
                                    FOR XML PATH(''), TYPE
                                ).value('.', 'NVARCHAR(MAX)'), 
                                1, 1, '')
                            ) AS strContractName,
                            -- 地点名称字符串（通过子查询连接获取施工地点）
                            (SELECT STUFF(
                                (
                                    SELECT ',' + COALESCE(s.name_eng, s.name_cht)
                                    FROM Biz_Contract c
                                    LEFT JOIN Biz_Site_Relationship sr ON c.id = sr.relation_id AND sr.relation_type = 0
                                    LEFT JOIN Biz_Site s ON sr.site_id = s.id
                                    WHERE c.project_id = p.id AND c.delete_status = 0 AND s.id IS NOT NULL
                                    GROUP BY s.id, COALESCE(s.name_eng, s.name_cht)
                                    FOR XML PATH(''), TYPE
                                ).value('.', 'NVARCHAR(MAX)'), 
                                1, 1, '')
                            ) AS strSiteName
                        FROM Biz_Project p
                        {strWhere}
                        ORDER BY p.create_date DESC";

                // 执行SQL查询获取所有数据
                var list = DBServerProvider.SqlDapper.QueryList<ProjectListDto>(sql, null);

                // 计算分页参数
                int page = dtoSearchInput.page_index > 0 ? dtoSearchInput.page_index : 1;
                int pageSize = dtoSearchInput.page_rows > 0 ? dtoSearchInput.page_rows : 20;
                int skip = (page - 1) * pageSize;

                // 执行内存分页
                pageGridData.data = list.Skip(skip).Take<ProjectListDto>(pageSize).ToList();
                pageGridData.status = true;
                pageGridData.total = list.Count; // 设置总行数

                // 处理返回的项目列表，确保数据完整性
                foreach (var item in pageGridData.data)
                {
                    // 获取该项目的合约和地点详细信息
                    var contractSql = $@"SELECT 
                                    c.id AS contract_id,
                                    c.contract_no,
                                    c.name_eng AS contract_name_eng,
                                    c.name_cht AS contract_name_cht,
                                    --c.issue_date,
                                    --c.end_date,
                                    s.id AS site_id,
                                    s.name_eng AS site_name_eng,
                                    s.name_cht AS site_name_cht
                                FROM Biz_Contract c
                                LEFT JOIN Biz_Site_Relationship sr ON c.id = sr.relation_id AND sr.relation_type = 0
                                LEFT JOIN Biz_Site s ON sr.site_id = s.id
                                WHERE c.project_id = '{item.id}' AND c.delete_status = 0 and s.id is not null";

                    var contractsData = DBServerProvider.SqlDapper.QueryList<dynamic>(contractSql, null);
                    var contractDict = new Dictionary<Guid, ProjectContractDto>();

                    // 构建合约和地点的关系
                    foreach (var contractItem in contractsData)
                    {
                        var contractId = contractItem.contract_id;
                        if (!contractDict.ContainsKey(contractId))
                        {
                            var contract = new ProjectContractDto
                            {
                                id = contractId,
                                contract_no = contractItem.contract_no,
                                name_eng = contractItem.contract_name_eng,
                                name_cht = contractItem.contract_name_cht,
                                //issue_date = contractItem.issue_date,
                                //end_date = contractItem.end_date,
                                sites = new List<ProjectContractSiteDto>()
                            };
                            contractDict[contractId] = contract;
                        }

                        var siteId = contractItem.site_id;
                        var siteDto = new ProjectContractSiteDto
                        {
                            id = siteId,
                            name_eng = contractItem.site_name_eng,
                            name_cht = contractItem.site_name_cht
                        };
                        contractDict[contractId].sites.Add(siteDto);
                    }

                    // 设置项目的合约列表
                    item.children = contractDict.Values.ToList();
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_ProjectService.GetProjectList", e);
                pageGridData.status = false;
                pageGridData.message = _localizationService.GetString("system_error");
            }
            return pageGridData;
        }

        #endregion
    }
}


using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Biz_SiteService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_SiteRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_SiteService(
            IBiz_SiteRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
             IMapper mapper,
            ILocalizationService localizationService
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        #region 新增、修改、删除、查询逻辑代码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="preQualInfo"></param>
        /// <returns></returns>
        public WebResponseContent AddSite(SiteDto dtoSite)
        {
            try
            {
                if (dtoSite == null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                }

                bool isExist = _repository.Exists(d => d.name_cht == dtoSite.name_cht && d.name_eng == dtoSite.name_eng);
                if (!isExist)
                {
                    Biz_Site bizSite = _mapper.Map<Biz_Site>(dtoSite);
                    bizSite.id = Guid.NewGuid();
                    bizSite.create_id = UserContext.Current.UserId;
                    bizSite.create_name = UserContext.Current.UserName;
                    bizSite.delete_status = (int)SystemDataStatus.Valid;
                    _repository.Add(bizSite, true);
                    string _msg = $"{_localizationService.GetString("save")}{_localizationService.GetString("success")}";
                    // return WebResponseContent.Instance.OK("Ok");
                    return WebResponseContent.Instance.OK(_msg);
                }
                //提示已存在
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("M_SiteService.AddSite 新增内容保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        /// <summary>
        /// 修改 Site
        /// </summary>
        /// <param name="mSiteDto"></param>
        /// <returns></returns>
        public WebResponseContent EditSite(SiteDto dtoSite)
        {
            try
            {
                if (dtoSite == null || dtoSite.id.IsNullOrEmpty())
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                }

                Biz_Site dbEntity = _repository.Find(d => d.id == dtoSite.id).FirstOrDefault();
                if (dbEntity != null)
                {
                    bool isExist = _repository.Exists(d => d.name_cht == dtoSite.name_cht && d.name_eng == dtoSite.name_eng);
                    if (!isExist)
                    {
                        MapValueToEntity(dtoSite, dbEntity);
                        dbEntity.create_id = UserContext.Current.UserId;
                        dbEntity.modify_name = UserContext.Current.UserName;
                        dbEntity.modify_date = DateTime.Now;
                        var effRow = _repository.Update(dbEntity, true);
                        return CheckOperationResult(effRow);
                    }
                    //提示已存在
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("existent")}");
                }
                //提示不存在
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("M_SiteService.AddSite 修改内容保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 失效 Site
        /// </summary>
        /// <returns></returns>
        public WebResponseContent DelSite(Guid guid)
        {
            try
            {
                if (guid.IsNullOrEmpty())
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                }

                var dbEntity = _repository.Find(d => d.id == guid && d.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                if (dbEntity != null)
                {
                    dbEntity.delete_status = (int)SystemDataStatus.Invalid;
                    dbEntity.modify_id = UserContext.Current.UserId;
                    dbEntity.modify_name = UserContext.Current.UserName;
                    dbEntity.modify_date = DateTime.Now;
                    var effRow = _repository.Update(dbEntity, true);
                    return CheckOperationResult(effRow);
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("M_SiteService.AddSite 失效操作保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        ///  检查结果是否成功
        /// </summary>
        /// <param name="effRow"></param>
        /// <returns></returns>
        private WebResponseContent CheckOperationResult(int affectRow)
        {
            if (affectRow > 0)
            {
                return WebResponseContent.Instance.OK($"{_localizationService.GetString("save")}{_localizationService.GetString("success")}");
            }
            else
            {
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("operation_failed")}");
            }
        }

        /// <summary>
        /// 获取Site列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteList(PageDataOptions pdo)
        {
            List<Biz_Site> lisSites = new List<Biz_Site>();
            try
            {

                //后续需要增加解析过滤条件
                var query = await _repository.FindAsIQueryable(x => x.delete_status == (int)SystemDataStatus.Valid).ToListAsync();
                var total = query.Count();
                //获取项目列表
                lisSites = await repository.FindAsIQueryable(x => x.delete_status == (int)SystemDataStatus.Valid)
                   .OrderByDescending(x => x.create_date)
                   .Skip((pdo.Page - 1) * pdo.Rows)
                   .Take(pdo.Rows)
                   .ToListAsync();

                return WebResponseContent.Instance.OK("Ok", lisSites);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this.GetType().ToString(), ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }

        }
        /// <summary>
        ///  根据Id 获取Site信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteById(Guid guid)
        {
            try
            {
                var dbEntiry = await _repository.FindAsync(d => d.delete_status == (int)SystemDataStatus.Valid && d.id == guid);
                return WebResponseContent.Instance.OK("OK", dbEntiry);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this.GetType().ToString(), ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion

        /// <summary>
        /// 现场地点下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSiteDownList()
        {
            try
            {
                var siteData = await _repository.FindAsync(p => p.delete_status == (int)SystemDataStatus.Valid);

                var siteDownList = new List<SiteDownDto>();
                foreach (var item in siteData)
                {
                    siteDownList.Add(new SiteDownDto
                    {
                        id = item.id,
                        name = item.name_sho
                    });
                }

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), siteDownList);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}

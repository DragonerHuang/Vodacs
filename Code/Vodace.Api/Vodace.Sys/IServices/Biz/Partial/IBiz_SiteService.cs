
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_SiteService
    {
        /// <summary>
        /// 添加Site
        /// </summary>
        /// <param name="mSiteDto"></param>
        /// <returns></returns>
        public WebResponseContent AddSite(SiteDto dtoSite);
        /// <summary>
        /// 修改 Site
        /// </summary>
        /// <param name="mSiteDto"></param>
        /// <returns></returns>
        public WebResponseContent EditSite(SiteDto dtoSite);
        /// <summary>
        /// 删除 Site
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public WebResponseContent DelSite(Guid guid);

        /// <summary>
        /// 获取Site信息列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetSiteList(PageDataOptions pdo);
        /// <summary>
        ///  根据Id 获取Site信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetSiteById(Guid guid);

        /// <summary>
        /// 现场地点下拉列表
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteDownList();
    }
 }


using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_ProjectService
    {
        WebResponseContent Add(ProjectDto projectDto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(ProjectDto projectDto);

        Task<WebResponseContent> GetProjectById(Guid guid);

        Task<WebResponseContent> GetProjectList(PageInput<SearchProjectDto> dtoSearchInput);

        Task<WebResponseContent> GetProjectContractList(PageInput<ContractSearchDto> searchDto);

        Task<WebResponseContent> GetProjectStatistics();

        Task<WebResponseContent> GetProjectAllName();

        /// <summary>
        /// 获取项目下拉列表
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> GetProjectDownListAsync(string proName = "");
    }
 }

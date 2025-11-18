
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_Construction_Content_InitService
    {
        WebResponseContent ImportData(IFormFile file);
        WebResponseContent Add(ConstructionContentInitDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(ConstructionContentInitEditDto dto);
        Task<WebResponseContent> GetContentInitList(PageInput<ConstructionContentInitEditDto> dto);
        Task<WebResponseContent> GetContentIntiWorkTypeList();
    }
 }

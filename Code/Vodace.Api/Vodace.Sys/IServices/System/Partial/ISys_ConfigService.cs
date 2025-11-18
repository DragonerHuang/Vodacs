using System;
using System.Threading.Tasks;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_ConfigService
    {
        Task<WebResponseContent> GetListByPage(PageInput<ConfigQuery> query);
        WebResponseContent Add(ConfigAddDto config);
        WebResponseContent Update(ConfigEditDto config);
        WebResponseContent Delete(Guid id);

        Task<WebResponseContent> GetConfigByTypeAsync(int configType, string congfigKey);
    }
 }

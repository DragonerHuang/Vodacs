using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.IServices
{
    public partial interface ISys_DictionaryListService
    {
        /// <summary>
        /// 根据code获取数字字典下拉列表
        /// </summary>
        /// <param name="dicCode"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetDictionaryByCode(string dicCode);
    }
 }


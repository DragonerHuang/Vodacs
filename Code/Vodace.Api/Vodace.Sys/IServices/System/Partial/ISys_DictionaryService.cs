using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.IServices
{
    public partial interface ISys_DictionaryService
    {
        /// <summary>
        /// 代码生成器获取所有字典项(超级管理权限)
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetBuilderDictionary();
        WebResponseContent GetVueDictionary(string[] dicNos);
        object GetTableDictionary(Dictionary<string, object[]> keyData);
        object GetSearchDictionary(string dicNo, string value);

        /// <summary>
        /// 表单设置为远程查询，重置或第一次添加表单时，获取字典的key、value
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<object> GetRemoteDefaultKeyValue(string dicNo, string key);

        Task<WebResponseContent> AddData(DictionaryDto dictionaryDto);
        Task<WebResponseContent> EditData(DictionaryEditDto dictionaryDto);
        Task<WebResponseContent> DelData(int id);
        Task<WebResponseContent> SwitchEnable(int id, int enable);
        WebResponseContent GetPageDataAsync(PageInput<DictionarySearchDto> search);
        WebResponseContent GetDataById(int id);
        Task<WebResponseContent> GetListByParent();
    }
}


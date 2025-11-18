using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Controllers.Basic;
using Vodace.Core.Extensions;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_DictionaryController
    {
        [HttpPost, Route("GetVueDictionary"), AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [ApiActionPermission()]
        public IActionResult GetVueDictionary([FromBody] string[] dicNos)
        {
            return Json(Service.GetVueDictionary(dicNos));
        }
        /// <summary>
        /// table加载数据后刷新当前table数据的字典项(适用字典数据量比较大的情况)
        /// </summary>
        /// <param name="value"></param> 
        /// <returns></returns>
        [HttpPost, Route("getTableDictionary")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetTableDictionary([FromBody] Dictionary<string, object[]> keyData)
        {
            return Json(Service.GetTableDictionary(keyData));
        }
        /// <summary>
        /// 远程搜索
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost, Route("getSearchDictionary"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetSearchDictionary(string dicNo, string value)
        {
            return Json(Service.GetSearchDictionary(dicNo, value));
        }

        /// <summary>
        /// 表单设置为远程查询，重置或第一次添加表单时，获取字典的key、value
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost, Route("getRemoteDefaultKeyValue"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetRemoteDefaultKeyValue(string dicNo, string key)
        {
            return Json(await Service.GetRemoteDefaultKeyValue(dicNo, key));
        }
        /// <summary>
        /// 代码生成器获取所有字典项(超级管理权限)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetBuilderDictionary")]
        [ApiExplorerSettings(IgnoreApi = true)]
        // [ApiActionPermission(ActionRolePermission.SuperAdmin)]
        public async Task<IActionResult> GetBuilderDictionary()
        {
            return Json(await Service.GetBuilderDictionary());
        }
        /// <summary>
        /// 新增数据字典
        /// </summary>
        /// <param name="dictionaryDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddData")]
        [ApiActionPermission]
        public async Task<IActionResult> AddData([FromBody] DictionaryDto dictionaryDto)
        {
            return Json(await Service.AddData(dictionaryDto));
        }
        /// <summary>
        /// 修改数据字典
        /// </summary>
        /// <param name="dictionaryDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditData")]
        [ApiActionPermission]
        public async Task<IActionResult> EditData([FromBody] DictionaryEditDto dictionaryDto)
        {
            return Json(await Service.EditData(dictionaryDto));
        }
        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelData")]
        [ApiActionPermission]
        public async Task<IActionResult> DelData(int id)
        {
            return Json(await Service.DelData(id));
        }


        /// <summary>
        /// 获取数据字典分页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("GetDicPageData")]
        [ApiActionPermission]
        public IActionResult GetDicPageDataAsync([FromBody] PageInput<DictionarySearchDto> search)
        {
            return Json(Service.GetPageDataAsync(search));
        }
        [HttpGet, Route("GetDataById"), ApiActionPermission]
        public IActionResult GetDataById(int id)
        {
            return Json(Service.GetDataById(id));
        }
        [HttpGet, Route("GetListByParent"), ApiActionPermission]
        public async Task<IActionResult> GetListByTree()
        {
            return Json(await Service.GetListByParent());

        }

        [HttpPut, Route("SwitchEnable"), ApiActionPermission]
        public async Task<IActionResult> SwitchEnable(int id, int enable)
        {
            return Json(await Service.SwitchEnable(id, enable));
        }
    }
}

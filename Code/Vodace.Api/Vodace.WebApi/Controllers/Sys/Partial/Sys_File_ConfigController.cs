
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Enums;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_File_ConfigController
    {
        private readonly ISys_File_ConfigService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_File_ConfigController(
            ISys_File_ConfigService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 新增文件配置
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        [HttpPost, Route("AddFileConfig")]
        [ApiActionPermission]
        public IActionResult AddFileConfig([FromBody] AddFileConfigDto dtoFileConfig)
        {
            return Json(Service.Add(dtoFileConfig));
        }

        /// <summary>
        /// 删除文件配置
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        /// <remarks>实际为软删除</remarks>
        [HttpDelete, Route("DelFileConfig")]
        [ApiActionPermission]
        public IActionResult DelFileConfig(Guid guid)
        {
            return Json(Service.Del(guid));
        }

        /// <summary>
        /// 修改文件配置
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        [HttpPut, Route("EditFileConfig")]
        [ApiActionPermission]
        public IActionResult EditFileConfig([FromBody] EditFileConfigDto dtoFileConfig)
        {
            return Json(Service.Edit(dtoFileConfig));
        }

        /// <summary>
        /// 获取文件配置列表
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果字段及说明：<br/>
        /// id：文件配置主键<br/>
        /// file_code：文件配置编码（目前后端代码控制，暂不提供对外开放）<br/>
        /// folder_path：存放的目录地址<br/>
        /// remark：备注<br/>
        /// create_name：创建人<br/>
        /// create_date：创建时间<br/>
        /// modify_name：修改人<br/>
        /// modify_date：修改时间
        /// </remarks>
        [HttpPost, Route("GetFileConfigList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetFileConfigList([FromBody] PageInput<AddFileConfigDto> dtoSearchInput)
        {
            return Json(await _service.GetFileConfigList(dtoSearchInput));
        }

        /// <summary>
        /// 获取文件配置详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet, Route("GetFileConfigById")]
        [ApiActionPermission]
        public async Task<IActionResult> GetFileConfigById(Guid guid)
        {
            return Json(await _service.GetFileConfigById(guid));
        }

        /// <summary>
        /// 获取文件配置名称及ID列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 返回结果字段及说明：<br/>
        /// id: 文件配置ID（GUID）<br />
        /// file_code：文件配置编码（目前后端代码控制，暂不提供对外开放）<br/>
        /// folder_path：存放的目录地址<br/>
        /// </remarks>
        [HttpGet, Route("GetFileConfigAllName")]
        [ApiActionPermission]
        public async Task<IActionResult> GetFileConfigAllName()
        {
            return Json(await _service.GetFileConfigAllName());
        }

    }
}

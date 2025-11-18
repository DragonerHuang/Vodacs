
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_ConfigController
    {
        private readonly ISys_ConfigService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_ConfigController(
            ISys_ConfigService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="query"></param>
        /// config_type：配置类型（0：消息提醒配置;1:文件存放路径）
        /// config_key：配置项键
        /// <returns></returns>
        [HttpPost,Route("GetListByPage"),ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody] PageInput<ConfigQuery> query) 
        {
            return Json(await _service.GetListByPage(query));
        }
        /// <summary>
        /// 新增配置项
        /// </summary>
        /// <param name="config"></param>
        /// config_type：配置类型（0：消息提醒配置;1:文件存放路径）
        /// config_key：配置项键
        /// config_value：配置项值
        /// <returns></returns>
        [HttpPost,Route("AddConfig"),ApiActionPermission]
        public IActionResult Add([FromBody]ConfigAddDto config) 
        {
            return Json(_service.Add(config));
        }
        /// <summary>
        /// 编辑配置项
        /// </summary>
        /// <param name="config"></param>
        /// id:id主键
        /// config_type：配置类型（0：消息提醒配置;1:文件存放路径）
        /// config_key：配置项键
        /// config_value：配置项值
        /// <returns></returns>
        [HttpPut, Route("EditConfig"), ApiActionPermission]
        public IActionResult Update([FromBody] ConfigEditDto config)
        {
            return Json(_service.Update(config));
        }
        [HttpDelete,Route("DelConfig"),ApiActionPermission]
        public IActionResult Delete(Guid id) 
        {
            return Json(_service.Delete(id));
        }

        /// <summary>
        /// 根据类型和key获取配置信息
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <param name="key">配置项键</param>
        /// <returns></returns>
        [HttpGet, Route("GetConfigByTypeAsync")]
        [ApiActionPermission]
        public async Task<IActionResult> GetConfigByTypeAsync(int config_type, string config_key)
        {
            return Json(await _service.GetConfigByTypeAsync(config_type, config_key));
        }
    }
}

/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_QuartzOptionsNew",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Api.Controllers.Hubs;
using Vodace.Core.Enums;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_QuartzOptionsController
    {
        private readonly ISys_QuartzOptionsService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<HomePageMessageHub> _hubContext;

        [ActivatorUtilitiesConstructor]
        public Sys_QuartzOptionsController(
            ISys_QuartzOptionsService service,
            IHttpContextAccessor httpContextAccessor,
            IHubContext<HomePageMessageHub> hubContext )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        /// <summary>
        /// api加上属性 [ApiTask]
        /// </summary>
        /// <returns></returns>
        [ApiTask]
        [HttpGet, HttpPost, Route("test2")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Test2()
        {
            //await _hubContext.Clients.User("3362").SendAsync("ReceiveHomePageMessage", "这是一条定时推送消息！");
            //await _hubContext.Clients.User("99999999-9999-9999-9999-999999999999").SendAsync("ReceiveHomePageMessage", "这是一条定时推送消息！");
            //await _hubContext.Clients.User("1").SendAsync("ReceiveHomePageMessage", "这是一条定时推送消息！");

            // var userId = _hub.GetCnnectionIds(username).ToArray();
            // _hubContext.Clients

            //await _hubContext.Clients.Users(username).SendAsync("ReceiveMessage", "消息推送1", "这是一条定时推送消息！");
            //await _hubContext.SendHomeMessage("admin666", "定时消息", "这是一条定时消息！");
            //await _hubContext.Clients.All.SendAsync("ReceiveHomePageMessage", "消息推送2", "这是一条定时消息！");
            Log4NetHelper.Info($"定时任务执行时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
            //_service.SendMsg("admin", "定时消息", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
            return Content(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }
        [ApiTask]
        [HttpGet, HttpPost, Route("PushMessage")]
        public IActionResult PushMessage(string userName) 
        {
            Log4NetHelper.Info($"定时任务推送消息，接收人：{userName}");
            //_service.SendMsg(userName, "定时消息", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
            return Content(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }


        /// <summary>
        /// api加上属性 [ApiTask]
        /// </summary>
        /// <returns></returns>
        [ApiTask]
        [HttpGet, HttpPost, Route("taskTest")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult TaskTest()
        {
            return Content(DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"));
        }

        /// <summary>
        /// 手动执行一次
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [Route("run"), HttpPost]
        [ActionPermission(ActionPermissionOptions.Update)]
        public async Task<object> Run([FromBody] Sys_QuartzOptions taskOptions)
        {
            return await Service.Run(taskOptions);
        }
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [Route("start"), HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ActionPermission(ActionPermissionOptions.Update)]
        public async Task<object> Start([FromBody] Sys_QuartzOptions taskOptions)
        {
            return await Service.Start(taskOptions);
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [Route("pause"), HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ActionPermission(ActionPermissionOptions.Update)]
        public async Task<object> Pause([FromBody] Sys_QuartzOptions taskOptions)
        {
            return await Service.Pause(taskOptions);
        }
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost, Route("GetListByPage"), ApiActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody]PageInput<TaskQuery> query) 
        {
            return Json(await _service.GetListByPage(query));
        }
        /// <summary>
        /// 定时任务-新增数据
        /// </summary>
        /// <param name="dto"></param>
        /// task_name：任务名称
        /// group_name：任务分组
        /// method：请求方式
        /// api_url：api接口地址
        /// post_data：post参数
        /// auth_key：认证key
        /// auth_value：认证value
        /// describe：描述
        /// status：运行状态
        /// time_out：超时时间(秒)
        /// cron_expression：Corn表达式
        /// <returns></returns>
        [HttpPut, Route("AddData")]
        [ApiActionPermission]
        public IActionResult Add([FromBody] TaskAddDto dto)
        {
            return Json(_service.Add(dto));
        }
        /// <summary>
        /// 定时任务-编辑数据
        /// </summary>
        /// <param name="dto"></param>
        /// task_name：任务名称
        /// group_name：任务分组
        /// method：请求方式
        /// api_url：api接口地址
        /// post_data：post参数
        /// auth_key：认证key
        /// auth_value：认证value
        /// describe：描述
        /// status：运行状态
        /// time_out：超时时间(秒)
        /// cron_expression：Corn表达式
        /// <returns></returns>
        [HttpPut, Route("EditData")]
        [ApiActionPermission]
        public IActionResult Update([FromBody] TaskEditDto dto) 
        {
            return Json(_service.Edit(dto));
        }
        /// <summary>
        /// 定时任务-切换状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut, Route("SwitchStatus")]
        [ApiActionPermission]
        public async Task<IActionResult> SwitchStatus(Guid id)
        {
            return Json(await _service.SwitchStatus(id));
        }
        /// <summary>
        /// 定时任务-删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DelData")]
        [ApiActionPermission]
        public IActionResult DelData(Guid id)
        {
            return Json(_service.DelData(id));
        }
        [HttpGet, Route("SendMsg"),AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult SendMsg(string userId, string title, string message) 
        {
            _service.SendMsg(userId, title, message);
            return Json("");
        }

    }
}

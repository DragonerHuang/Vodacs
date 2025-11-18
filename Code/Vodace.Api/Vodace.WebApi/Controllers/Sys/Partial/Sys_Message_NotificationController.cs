
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_Message_NotificationController
    {
        private readonly ISys_Message_NotificationService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Message_NotificationController(
            ISys_Message_NotificationService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        [ApiTask]
        [HttpGet,Route("CheckEventTask"),ApiActionPermission]
        public IActionResult CheckEventTask() 
        {
            return Json(Service.CheckEventTask());
        }
        /// <summary>
        /// 获取消息通知列表
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        [HttpGet, Route("GetList"), ApiActionPermission]
        public IActionResult  GetList(string user_name) 
        {
            return Json(_service.GetList(user_name));
        }
        [HttpGet,Route("SendEmail"),ApiActionPermission]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult SendEmail(string title, string content, params string[] list) 
        {
            MailHelper.SendEmail(title, content, list);
            return Json("Ok");
        }
        /// <summary>
        /// 更新消息状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">消息状态（0：未读；1：已读；2：已处理；3：已逾期；4：过期）</param>
        /// <returns></returns>
        [HttpPut,Route("UpdateStatus"),ApiActionPermission]
        public IActionResult UpdateStatus(Guid id, int status)
        {
            return Json(_service.UpdateStatus(id,status));
        }
        [ApiTask]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet,Route("PushMessageToUser")]
        public async Task<IActionResult> PushMessageToUser() 
        {
            return Json(await _service.PushMessageToUser());
        }
        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet, Route("UserOffline")]
        public async Task<IActionResult>  UserOffline(string userName)
        {
            return Json(await _service.UserOffline(userName));
        }
        //[HttpGet, Route("SendMailOutLook"),AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<IActionResult> SendMail(string sRecipientEmail, string sSubject, string sMessage)
        //{
        //    return Json(await MailHelperOutLook.SendMailOutLook(sSubject, sMessage,sRecipientEmail));
        //}
    }
}

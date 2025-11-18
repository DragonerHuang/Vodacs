
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;
using Vodace.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace Vodace.Sys.Controllers
{
    public partial class Sys_User_RegisterController
    {
        private readonly ISys_User_RegisterService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_User_RegisterController(
            ISys_User_RegisterService service,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 用户注册（流程1）
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost, Route("AddUserRegister"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult AddUserRegister([FromBody] UserBaseDto register) 
        {
            return Json(_service.AddUserRegister(register));
        }
        /// <summary>
        /// 用户注册(完整流程)
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost, Route("RegisterUserNew"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult RegisterUserNew([FromBody] UserRegisterNewDto register) 
        {
            return Json(_service.RegisterUserNew(register));
        }

        //[HttpPost, Route("RegisterUser"),AllowAnonymous]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public IActionResult RegisterUser([FromBody]UserRegisterDto register) 
        //{
        //    return Json(_service.RegisterUser(register));
        //}
        /// <summary>
        /// 验证注册身份证号
        /// </summary>
        /// <param name="idNo"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet, Route("CheckExist"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CheckExist(string idNo) 
        {
            return Json(await _service.CheckExist(idNo));
        }

        /// <summary>
        /// 注册时验证公司是否存在
        /// </summary>
        /// <param name="com_no"></param>
        /// <returns></returns>
        [HttpGet, Route("CheckCompanyNo"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CheckCompanyNo(string com_no)
        {
            return Json(await _service.CheckCompanyNo(com_no));
        }

        /// <summary>
        /// 注册时验证用户名是否存在
        /// </summary>
        /// <param name="com_no"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet, Route("CheckUserExist"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CheckUserExist(string com_no, string userName)
        {
            return Json(await _service.CheckUserExist(com_no, userName));
        }
    }
}

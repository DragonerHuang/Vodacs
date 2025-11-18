using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.Utilities.Log4Net;

namespace Vodace.Core.Filters
{
    /// <summary>
    /// 全局异常错误日志
    /// </summary>
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        //private readonly ILoggerHelper _loggerHelper;
        public GlobalExceptionsFilter(IWebHostEnvironment env)
        {
            _env = env;
            //_loggerHelper = loggerHelper;
        }
        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            json.Message = context.Exception.Message;//错误信息
            string info = $@"StatusCode:{context.HttpContext.Response.StatusCode}";
            string remoteIpAddr = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (_env.IsDevelopment())
            {
                json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
            }
            context.Result = new InternalServerErrorObjectResult(json);

            //采用log4net 进行错误日志记录
            Log4NetHelper.Error(
                  $@"HTTP-OnException->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                  info + "->" + json.Message + "->" + context.Exception.StackTrace);
        }

    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
    //返回错误信息
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }
}

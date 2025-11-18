using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.ObjectActionValidator;
using Vodace.Core.Services;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;

namespace Vodace.Core.Filters
{
    public class ActionExecuteFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string remoteIpAddr = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (remoteIpAddr.Contains(":"))
            {
                var arr = remoteIpAddr.Split("f:");
                remoteIpAddr = arr.Length > 1 ? arr[1] : "";
            }
            try
            {
                if (context.HttpContext.Request.Path.Value.Contains("api/") || context.HttpContext.Request.Path.Value.Contains("api/"))
                {
                    if (context.HttpContext.Request.Path.Value.Contains("api/"))
                    {
                        //Log4NetHelper.Debug(
                        //$@"HTTP-INPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                        //   $@"{JsonHelper.ToJson(context.ActionArguments)}");
                    }
                }
                else
                {
                    if (context.HttpContext.Request.Path.Value.Contains("api/"))
                    {
                        //Log4NetHelper.Debug(
                        //   $@"HTTP-INPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                        //      $@"{JsonHelper.ToJson(context.ActionArguments)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(
                    $@"HTTP-INPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                    remoteIpAddr + "->" + ex.Message + "->" + ex.StackTrace);
            }
            //验证方法参数
            context.ActionParamsValidator();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            string info = $@"StatusCode:{context.HttpContext.Response.StatusCode}";
            string remoteIpAddr = context.HttpContext.Connection.RemoteIpAddress.ToString();
            if (remoteIpAddr.Contains(":"))
            {
                var arr = remoteIpAddr.Split("f:");
                remoteIpAddr = arr.Length > 1 ? arr[1] : "";
            }
            try
            {
                if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    if (context.HttpContext.Request.Path.Value.Contains("api/") || context.HttpContext.Request.Path.Value.Contains("api/"))
                    {
                        //info =
                        //   $@"{info}->Body: {JsonHelper.ToJson(context.Result)}";
                        //Log4NetHelper.Debug(
                        //    $@"HTTP-OUTPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                        //    info);
                    }

                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(
                    $@"HTTP-OUTPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                    info + "->" + ex.Message + "->" + ex.StackTrace);
            }
        }
    }
}
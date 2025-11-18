using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;

namespace Vodace.Core.Filters
{
    public class LogAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// 请求后
        /// </summary>
        /// <param name="context"></param>
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
                    if (context.HttpContext.Request.Path.Value.Contains("api/Weightdata") || context.HttpContext.Request.Path.Value.Contains("api/customer"))
                    {
                        info =
                           $@"{info}->Body: {JsonHelper.ToJson(context.Result)}";
                        Log4NetHelper.Debug(
                            $@"HTTP-OUTPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                            info);
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

        /// <summary>
        /// 请求中
        /// </summary>
        /// <param name="context"></param>
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
                if (context.HttpContext.Request.Path.Value.Contains("api/Weightdata") || context.HttpContext.Request.Path.Value.Contains("api/customer"))
                {
                    if (context.HttpContext.Request.Path.Value.Contains("api/Weightdata"))
                    {
                        Log4NetHelper.Debug(
                        $@"HTTP-INPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                           $@"{JsonHelper.ToJson(context.ActionArguments)}");
                    }
                }
                else
                {
                    if (context.HttpContext.Request.Path.Value.Contains("api/Weightdata"))
                    {
                        Log4NetHelper.Debug(
                           $@"HTTP-INPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                              $@"{JsonHelper.ToJson(context.ActionArguments)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(
                    $@"HTTP-INPUT->{remoteIpAddr}->{context.HttpContext.Request.Method}->{context.HttpContext.Request.Path}->" +
                    remoteIpAddr + "->" + ex.Message + "->" + ex.StackTrace);
            }
        }
    }
}

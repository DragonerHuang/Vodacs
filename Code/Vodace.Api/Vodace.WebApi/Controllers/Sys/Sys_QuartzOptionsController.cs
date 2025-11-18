/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_QuartzOptionsController编写
 */
using Microsoft.AspNetCore.Mvc;
using Vodace.Core.Controllers.Basic;
using Vodace.Entity.AttributeManager;
using Vodace.Sys.IServices;
namespace Vodace.Sys.Controllers
{
    [PermissionTable(Name = "Sys_QuartzOptionsNew")]
    [Route("api/v{version:apiVersion}/QuartzOptions")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public partial class Sys_QuartzOptionsController : ApiBaseController<ISys_QuartzOptionsService>
    {
        public Sys_QuartzOptionsController(ISys_QuartzOptionsService service)
        : base(service)
        {
        }
    }
}


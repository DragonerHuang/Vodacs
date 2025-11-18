using Autofac.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Configuration;
using Vodace.Core.Controllers.Basic;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity.AttributeManager;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;
using Vodace.Sys.IServices.System.Partial;
using Vodace.Sys.Services.System.Partial;

namespace Vodace.Api.Controllers.Sys.Partial
{
    [Route("api/v{version:apiVersion}/SysFile")]
    [ApiVersion("1.0")]
    public partial class Sys_FileController : VolController
    {
        private readonly ISys_FileService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_FileController(
            ISys_FileService service,
            IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
        }

        /// <summary>
        /// 读取文件夹下的所有文件及文件夹
        /// </summary>
        /// <param name="strDirectoryPath">读取文件夹的路径</param>
        /// <returns></returns>
        /// <remarks>
        /// 参数说明：<br/>
        /// strDirectoryPath：需要访问的文件地址，默认提取运行目录下地址<br/>
        /// <br/>
        /// 返回参数结果说明：<br/>
        /// Name：文件(夹)名称<br/>
        /// FileSize：文件大小<br/>
        /// FileType：文件类型<br/>
        /// FullPath：文件(夹)完整路径<br/>
        /// IsDirectory：是否目录<br/>
        /// </remarks>
        [HttpPost, Route("GetDirectoryContentsxx"), AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<IActionResult> GetDirectoryContentsxx(string strDirectoryPath = "")
        {
            if (string.IsNullOrEmpty(strDirectoryPath))
                strDirectoryPath = AppSetting.FileSaveSettings.FolderPath;

            FileHelper fileHelper = new FileHelper();
            var items = fileHelper.GetDirectoryContents(strDirectoryPath);
            List<FileSystemItem> result = new List<FileSystemItem>();
            foreach (FileSystemItem item in items)
            {
                result.Add(new FileSystemItem() {
                    Name = item.Name,
                    FileSize = item.FileSize,
                    FileType = item.FileType,
                    FullPath = item.FullPath,
                    IsDirectory = item.IsDirectory
                });
            }

            return Json(result);
        }

        /// <summary>
        /// 读取文件夹下的所有文件及文件夹
        /// </summary>
        /// <param name="strDirectoryPath">读取文件夹的路径</param>
        /// <param name="qn_no">qn编码</param>
        /// <returns></returns>
        /// <remarks>
        /// 参数说明：<br/>
        /// strDirectoryPath：需要访问的文件地址，默认提取运行目录下地址<br/>
        /// qn_no：报价编码<br/>
        /// qn_id：报价Id<br/>
        /// <br/>
        /// 返回参数结果说明：<br/>
        /// Name：文件(夹)名称<br/>
        /// FileSize：文件大小<br/>
        /// FileType：文件类型<br/>
        /// FullPath：文件(夹)完整路径<br/>
        /// IsDirectory：是否目录<br/>
        /// </remarks>
        [HttpPost, Route("GetDirectoryContents")]
        [ApiActionPermission]
        public async Task<IActionResult> GetDirectoryContents([FromBody] GetFileInfoDto dto)
        {
            return Json(await _service.GetDirectoryContents(dto));
        }
    }
}

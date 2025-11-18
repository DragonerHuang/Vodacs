
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
    public partial class Biz_Quotation_InterviewController
    {
        private readonly IBiz_Quotation_InterviewService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_InterviewController(
            IBiz_Quotation_InterviewService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 查询招标面试列表（分页）
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetQnInterviewPage"), ActionPermission]
        public async Task<IActionResult> GetQnInterviewPageAsync([FromBody] PageInput<QnInterviewSearchDto> pageInput)
        {
            return Json(await _service.GetQnInterviewPageAsync(pageInput));
        }

        /// <summary>
        /// 查询招标面试列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("GetQnInterview"), ActionPermission]
        public async Task<IActionResult> GetQnInterviewAsync([FromBody] QnInterviewSearchDto input)
        {
            return Json(await _service.GetQnInterviewAsync(input));
        }

        /// <summary>
        /// 上传完成文件，放置在临时目录
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost, Route("UpLoadFinishFile"), ActionPermission]
        public async Task<IActionResult> UpLoadFinishFile(List<IFormFile> files)
        {
            return Json(await _service.UpLoadFinishFile(files));
        }

        /// <summary>
        /// 创建招标面试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("AddQnInterview"), ActionPermission]
        public async Task<IActionResult> AddQnInterviewAsync([FromBody] AddQnInterview input)
        {
            return Json(await _service.AddQnInterviewAsync(input));
        }

        /// <summary>
        /// 编辑招标面试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut, Route("EditQnInterview"), ActionPermission]
        public async Task<IActionResult> EditQnInterviewAsync([FromBody] EditQnInterview input)
        {
            return Json(await _service.EditQnInterviewAsync(input));
        }

        /// <summary>
        /// 删除招标面试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteQnInterview"), ActionPermission]
        public async Task<IActionResult> DeleteQnInterviewAsync(Guid id)
        {
            return Json(await _service.DeleteQnInterviewAsync(id));
        }

        /// <summary>
        /// 编辑回复日期
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("EditReplyDate"), ActionPermission]
        public async Task<IActionResult> EditReplyDateAsync([FromBody] QnInterviewSearchDto input)
        {
            return Json(await _service.EditReplyDateAsync(input));
        }
    }
}

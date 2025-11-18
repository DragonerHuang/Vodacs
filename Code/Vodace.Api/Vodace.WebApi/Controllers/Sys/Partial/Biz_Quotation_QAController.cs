
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
    public partial class Biz_Quotation_QAController
    {
        private readonly IBiz_Quotation_QAService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_QAController(
            IBiz_Quotation_QAService service,
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
        /// <returns></returns>
        [HttpPost,Route("GetListByPage"),ActionPermission]
        public async Task<IActionResult> GetListByPage([FromBody] PageInput<QuotationQA_Query> query) 
        {
            return JsonNormal(await _service.GetDataByPage(query));
        }
        /// <summary>
        /// 新增问答
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <returns></returns>
        [HttpPost,Route("AddData"),ActionPermission]
        public IActionResult AddData([FromBody]QuotationQADto quotationQADto) 
        {
            return Json(_service.AddData(quotationQADto));
        }
        /// <summary>
        /// 新增问答同上传文件一起提交
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost, Route("AddDataWithFile"), ActionPermission]
        public IActionResult AddDataWithFile([FromForm] QuotationQAWithFileAddDto quotationQADto, IFormFile file = null) 
        {
            return Json(_service.AddDataWithFile(quotationQADto, file));
        }
        /// <summary>
        /// 编辑问答同上传一起提交
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPut, Route("EditDataWithFile"), ActionPermission]
        public IActionResult EditDataWithFile([FromForm] QuotationQAWithFileEditDto quotationQADto, IFormFile file = null) 
        {
            return Json(_service.EditDataWithFile(quotationQADto, file));
        }
        /// <summary>
        /// 编辑问答
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditData"), ApiActionPermission]
        public IActionResult EditData([FromBody] QuotationQADto quotationQADto)
        {
            return Json(_service.EditData(quotationQADto));
        }
        /// <summary>
        /// 删除问答
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete,Route("DelData"),ApiActionPermission]
        public async Task<IActionResult> DelData(Guid id) 
        {
            //return Json(_service.DelData(id));
            return Json(await _service.DeleteQAAsync(id));
        }
        /// <summary>
        /// 上传问答文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="intSubmitType">问答类型（0：预审问答；1：投标问答）</param>
        /// <param name="guidQaId"></param>
        /// <returns></returns>
        [HttpPost, Route("UploadQAFile"), ApiActionPermission]
        public async Task<IActionResult> UploadQAFile(IFormFile file, [FromForm] int intSubmitType, [FromForm] Guid guidQaId) 
        {
            return Json(await _service.UploadQAFile(file, intSubmitType, guidQaId));
        }
        /// <summary>
        /// <summary>
        /// 下载问答文件
        /// </summary>
        /// <param name="qaId"></param>
        /// <returns></returns>
        [HttpGet,Route("DownloadSysQAFile"), ApiActionPermission]
        public async Task<IActionResult> DownloadSysQAFile(Guid qaId, int SubmitType)
        {
            var result = await _service.DownloadSysQAFile(qaId, SubmitType);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;
            return File(file.file_bytes, file.cntent_type, file.file_name);
        }
        /// <summary>
        /// 下载全部问答文件(压缩包)
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("DownLoadFiles"),ApiActionPermission]
        public async Task<IActionResult>  DownLoadFiles(Guid qnid, int type)
        {
            var result = await _service.DownLoadFiles(qnid, type);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;
            return File(file.file_bytes, file.cntent_type, file.file_name);
        }



        /// <summary>
        /// 新增问答记录
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddQAData"), ActionPermission]
        public async Task<IActionResult> AddQADataAsync([FromBody] QuotationQAWithFileAddDto quotationQADto)
        {
            return Json(await _service.AddQADataAsync(quotationQADto));
        }

        /// <summary>
        /// 修改问答记录
        /// </summary>
        /// <param name="quotationQADto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditQAData"), ActionPermission]
        public async Task<IActionResult> EditQADataAsync([FromBody] EditQuotationQADto quotationQADto)
        {
            return Json(await _service.EditQADataAsync(quotationQADto));
        }

        /// <summary>
        /// 根据id获取问答记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetQADataById"), ActionPermission]
        public async Task<IActionResult> GetQADataByIdAsync(Guid id)
        {
            return Json(await _service.GetQADataByIdAsync(id));
        }

        /// <summary>
        /// 问答文件上传
        /// </summary>
        /// <param name="lstFiles">文件</param>
        /// <param name="type">问答类型（0：预审问答；1：投标问答）</param>
        /// <param name="qn_id"></param>
        /// <returns></returns>
        [HttpPost, Route("UploadQAFiles"), ApiActionPermission]
        public async Task<IActionResult> UploadQAFilesAsync(List<IFormFile> lstFiles, [FromForm] Guid qn_id, [FromForm] int type)
        {
            return Json(await _service.UploadQAFilesAsync(lstFiles, qn_id, type));
        }

        /// <summary>
        /// 问答完成文件上传
        /// </summary>
        /// <param name="lstFiles">文件</param>
        /// <param name="type">问答类型（0：预审问答；1：投标问答）</param>
        /// <param name="qn_id"></param>
        /// <returns></returns>
        [HttpPost, Route("UploadQAFinishFiles"), ApiActionPermission]
        public async Task<IActionResult> UploadQAFinishFilesAsync(List<IFormFile> lstFiles, [FromForm] Guid qn_id, [FromForm] int type)
        {
            return Json(await _service.UploadQAFinishFilesAsync(lstFiles, qn_id, type));
        }

        /// <summary>
        /// 根据id下载问答文件
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("DownLoadQAFilesById"), ApiActionPermission]
        public async Task<IActionResult> DownLoadQAFilesByIdAsync(Guid id)
        {
            var result = await _service.DownLoadQAFilesByIdAsync(id);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;
            return File(file.file_bytes, file.cntent_type, file.file_name);
        }

        /// <summary>
        /// 下载全部问答
        /// </summary>
        /// <param name="type">问答类型（0：预审问答；1：投标问答）</param>
        /// <param name="qn_id"></param>
        /// <returns></returns>
        [HttpGet, Route("DownLoadAllQAFiles"), ApiActionPermission]
        public async Task<IActionResult> DownLoadAllQAFilesAsync(Guid qn_id, int type)
        {
            var result = await _service.DownLoadAllQAFilesAsync(qn_id, type);
            if (!result.status)
            {
                return Json(result);
            }
            var file = result.data as FileDownLoadDto;
            return File(file.file_bytes, file.cntent_type, file.file_name);
        }
    }
}

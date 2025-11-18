
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Filters;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Controllers
{
    public partial class Biz_QuotationController
    {
        private readonly IBiz_QuotationService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Biz_QuotationController(
            IBiz_QuotationService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 合同信息创建时上传文件接口
        /// </summary>
        /// <param name="litFiles"></param>
        /// <param name="strFileType"></param>
        /// <returns></returns>
        [HttpPost, Route("ContractUpload")]
        [ApiActionPermission]
        public async Task<IActionResult> AddContractUploadAsync(
            List<IFormFile> litFiles)
        {
            var result = await _service.UploadContractFiles(litFiles);
            return Json(result);
        }


        /// <summary>
        /// 创建报价
        /// </summary>
        /// <param name="addQnRequestDto"></param>
        /// <returns></returns>
        [HttpPost, Route("AddQn")]
        [ApiActionPermission]
        public IActionResult AddQn([FromBody] AddQnRequestDto addQnRequestDto)
        {
            var result = _service.AddQn(addQnRequestDto);

            return Json(result);
        }

        /// <summary>
        /// 修改报价
        /// </summary>
        /// <param name="editQnRequestDto"></param>
        /// <returns></returns>
        [HttpPut, Route("EditQn")]
        [ApiActionPermission]
        public IActionResult EditQn([FromBody] EditQnRequestDto editQnRequestDto)
        {
            var result = _service.EditQn(editQnRequestDto);

            return Json(result);
        }

        /// <summary>
        /// 删除报价
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteQn")]
        [ApiActionPermission]
        public IActionResult DeleteQn(Guid qnId)
        {
            var result = _service.DelQn(qnId);

            return Json(result);
        }

        /// <summary>
        /// 获取报价列表（分页）
        /// </summary>
        /// <param name="dtoQnSearchInput"></param>
        /// <returns></returns>
        [HttpPost, Route("GetQnPageData")]
        [ApiActionPermission]
        public async Task<IActionResult> GetQnPageData([FromBody]PageInput<QnSearchDto> dtoQnSearchInput)
        {
            var result = await _service.GetQnPageListAsync(dtoQnSearchInput);
            return Json(result);
        }

        /// <summary>
        /// 获取报价统计
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetQnSumData")]
        [ApiActionPermission]
        public async Task<IActionResult> GetQnSumData()
        {
            var result = await _service.CountQnAmtAsync();
            return Json(result);
        }

        /// <summary>
        /// 获取报价详细信息
        /// </summary>
        /// <param name="queryQn"></param>
        /// <returns></returns>
        [HttpGet, Route("GetQnDetailById")]
        [ApiActionPermission]
        public async Task<IActionResult> GetQnDetailById(Guid qnId)
        {
            var result = await _service.GetQnByIdAsync(qnId);
            return Json(result);
        }

        /// <summary>
        /// 获取父项报价下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetContractDropList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetMasterContractDropList(Guid id)
        {
            var result = await _service.GetQnDropList(true, id);
            return Json(result);
        }
        [HttpGet, Route("GetContractDropListNew")]
        [ApiActionPermission]
        public async Task<IActionResult> GetContractDropList()
        {
            var result = await _service.GetQnDropList();
            return Json(result);
        }

        /// <summary>
        /// 获取子项报价下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetChildContractDropList")]
        [ApiActionPermission]
        public async Task<IActionResult> GetChildContractDropList(Guid id)
        {
            var result = await _service.GetQnDropList(false, id);
            return Json(result);
        }

        /// <summary>
        /// 上传提交文件
        /// </summary>
        /// <param name="lstFiles">文件集合</param>
        /// <param name="intSubmitType">文件类型（0：财务类文件、1：技术类文件）</param>
        /// <param name="guidQnId">所属qn的id</param>
        /// <returns></returns>
        [HttpPost, Route("UploadSubmitFile")]
        [ApiActionPermission]
        public async Task<IActionResult> UploadSubmitFile(List<IFormFile> lstFiles, [FromForm] int intSubmitType, [FromForm] Guid guidQnId)
        {
            var result = await _service.UploadSubmitFile(lstFiles, intSubmitType, guidQnId);
            return Json(result);
        }

        /// <summary>
        /// 下载提交文件
        /// </summary>
        /// <param name="guidQnId"></param>
        /// <returns></returns>
        [HttpPost, Route("DownloadSubmitFile")]
        [ApiActionPermission]
        public async Task<IActionResult> DownloadSubmitFile(Guid guidQnId)
        {
            var result = await _service.DownSubmitFile(guidQnId);
            if (!result.status)
            {
                return Json(result);
            }

            var file = result.data as FileDownLoadDto;

            return File(file.file_bytes, file.cntent_type, file.file_name);
        }

        /// <summary>
        /// 提交报价文件
        /// </summary>
        /// <param name="guidQnId"></param>
        /// <returns></returns>
        [HttpPost, Route("SubmitQnFiles")]
        [ApiActionPermission]
        public async Task<IActionResult> SubmitQnFiles(Guid guidQnId)
        {
            var result = await _service.SubmitQnFiles(guidQnId);
            return Json(result);
        }

        /// <summary>
        /// 获取报价状态下拉列表
        /// </summary>
        /// <param name="qnId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetStatuCodeByQnId")]
        [ApiActionPermission]
        public async Task<IActionResult> GetStatuCodeByQnId(Guid qnId)
        {
            var result = await _service.GetStatuCodeByQnId(qnId);
            return Json(result);
        }

        /// <summary>
        /// 获取提交文件列表
        /// </summary>
        /// <param name="qn_id">所属qn的id</param>
        /// <param name="type">文件类型（0：财务类文件、1：技术类文件）</param>
        /// <returns></returns>
        [HttpGet, Route("GetSumitFiles")]
        [ApiActionPermission]
        public async Task<IActionResult> GetSumitFilesAync(Guid qn_id, int type)
        {
            var result = await _service.GetSumitFilesAync(qn_id, type);
            return Json(result);
        }

        /// <summary>
        /// 根据id删除提交文件
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteSumitFile")]
        [ApiActionPermission]
        public async Task<IActionResult> DeleteSumitFileAsync(Guid id)
        {
            var result = await _service.DeleteSumitFileAsync(id);
            return Json(result);
        }

        /// <summary>
        /// 提交完成文件
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPost, Route("SumbitFinishFiles")]
        [ApiActionPermission]
        public async Task<IActionResult> SumbitFinishFilesAsync(List<IFormFile> lstFiles, [FromForm] Guid qn_id)
        {
            var result = await _service.SumbitFinishFilesAsync(lstFiles, qn_id);
            return Json(result);
        }
    }
}

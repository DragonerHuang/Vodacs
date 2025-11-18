
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Site_Work_RecordService
    {
        /// <summary>
        /// 分页查询工地工作记录
        /// </summary>
        /// <param name="pageInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteWorkRecordPageAsync(PageInput<SiteWorkRecordSearchDto> pageInput);

        /// <summary>
        /// 根据时间获取工地工作记录
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteWorkRecordByDateAsync(SiteWorkRecordSearchDto search);

        /// <summary>
        /// 根据id获取工地记录详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetSiteWorkRecordByIdAsync(Guid id);

        /// <summary>
        /// 编辑工地工作记录
        /// </summary>
        /// <param name="editInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> EditSiteWorkRecordAsync(EditSiteWorkRecordDto editInput);

        /// <summary>
        /// 删除工地工作记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResponseContent> DeleteSiteWorkRecordByIdAsync(Guid id);

        /// <summary>
        /// 设置工程进度完成百分比
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WebResponseContent> SetProjectProgressAsync(SetProgressDto input);

        /// <summary>
        /// 获取工程进度完成百分比
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetProjectProgressAsync(Guid recordId);

        /// <summary>
        /// 根据工地记录id获取图片列表
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetPhotoListByRecordAsync(Guid recordId);

        /// <summary>
        /// 根据工地记录和类型获取图片列表
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="photoType"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetPhotoListByTypeAsync(Guid recordId, int photoType);

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="photoType"></param>
        /// <param name="lstFiles"></param>
        /// <returns></returns>
        Task<WebResponseContent> UploadPhotoAsync(Guid recordId, int photoType, List<IFormFile> lstFiles);

        /// <summary>
        /// 获取滚动计划任务进度数据
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetProjectProgressTableDataAsync(Guid contractId, Guid taskId);

        /// <summary>
        /// 获取滚动计划进度数据
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetRollingProgramScheduleDataAsync(Guid contractId, Guid taskId);

    }
 }

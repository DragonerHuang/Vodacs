using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vodace.Core.Configuration;
using Vodace.Core.DBManager;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IServices.System.Partial;

namespace Vodace.Sys.Services.System.Partial
{
    public partial class Sys_FileService : ISys_FileService, IDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILocalizationService _localizationService;

        [ActivatorUtilitiesConstructor]
        public Sys_FileService(
            ILocalizationService localizationService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
        }

        public async Task<WebResponseContent> GetDirectoryContents(GetFileInfoDto dto)
        {
            try
            {
                string strDirectoryPath = string.Empty;
                var programFiles = AppSetting.FileSaveSettings.FolderPath;

                if (!string.IsNullOrEmpty(dto.qn_id.ToString()) || !string.IsNullOrEmpty(dto.qn_no))
                {
                    var dbContext = DBServerProvider.DbContext;
                    var data = dbContext.Set<Biz_Quotation>()
                        //.Where(a => a.qn_no == dto.qn_no)
                        .WhereIF(!string.IsNullOrEmpty(dto.qn_no), q => q.qn_no == dto.qn_no)
                        .WhereIF(dto.qn_id.ToString() != "", q => q.id == dto.qn_id)
                        .GroupJoin(
                            dbContext.Set<Biz_Contract>(),
                            q => q.contract_id,
                            bc => bc.id,
                            (q, bcList) => new { q, bcList }
                        )
                        .SelectMany(
                            x => x.bcList.DefaultIfEmpty(), // 实现左连接
                            (x, bc) => new { x.q, bc }
                        )
                        .GroupJoin(
                            dbContext.Set<Sys_Company>(),
                            x => x.bc.company_id, // 使用空条件运算符处理bc为null的情况
                            c => c.id,
                            (x, cList) => new { x.q, x.bc, cList }
                        )
                        .SelectMany(
                            x => x.cList.DefaultIfEmpty(), // 实现左连接
                            (x, c) => new
                            {
                                company_no = c.company_no,
                                qn_no = x.q.qn_no,
                                contract_no = x.bc.contract_no,
                                company_id = x.bc.company_id,
                                create_date = x.q.create_date
                            }
                        )
                        .FirstOrDefault();
                    if (data != null)
                    {
                        var config = dbContext.Set<Sys_Config>().Where(a => a.config_key == "HKFiscalYear").FirstOrDefault()?.config_value;

                        var time = (DateTime)data.create_date;
                        int intBeginYear = 0;
                        int intEndYear = 0;
                        var year = time.Year;
                        var month = time.Month;

                        if (config == "1")
                        {
                            if (month >= 4)
                            {
                                intBeginYear = year;
                                intEndYear = year + 1;
                            }
                            else
                            {
                                intBeginYear = year - 1;
                                intEndYear = year;
                            }
                        }
                        else
                        {
                            intBeginYear = year;
                            intEndYear = year + 1;
                        }

                        //{配置文件地址}/{公司编码}/Project {年份-年份+1}/{报价编码}/
                        strDirectoryPath = Path.Combine(
                            programFiles,
                            data.company_no,
                            $"Project {intBeginYear}-{intEndYear}",
                            data.qn_no
                        );

                        //新扩展要求（必须同时填写qn信息以及目录地址情况下的校验）
                        if (!string.IsNullOrEmpty(dto.strDirectoryPath))
                        {
                            if (!dto.strDirectoryPath.Contains(data.qn_no))
                            {
                                return WebResponseContent.Instance.Error(_localizationService.GetString("quotation_directory_null"));
                            }
                            else
                            {
                                strDirectoryPath = programFiles + dto.strDirectoryPath;
                            }
                        }

                    }else
                        return WebResponseContent.Instance.Error(_localizationService.GetString("quotation_null"));
                }
                else
                {
                    //if (string.IsNullOrEmpty(strDirectoryPath))
                    //    strDirectoryPath = AppSetting.FileSaveSettings.FolderPath;

                    //return WebResponseContent.Instance.Error(_localizationService.GetString("qn_id_null"));

                    strDirectoryPath = programFiles + dto.strDirectoryPath;
                }

                if (!string.IsNullOrEmpty(strDirectoryPath))
                {
                    FileHelper fileHelper = new FileHelper();
                    var items = fileHelper.GetDirectoryContents(strDirectoryPath);
                    if (items.Count > 0)
                    {
                        List<FileSystemItem> result = new List<FileSystemItem>();
                        foreach (FileSystemItem item in items)
                        {
                            result.Add(new FileSystemItem()
                            {
                                Name = item.Name,
                                FileSize = item.FileSize,
                                FileType = item.FileType,
                                FullPath = item.FullPath.Replace(programFiles, ""), //目前提取数据，指定目录
                                IsDirectory = item.IsDirectory
                            });
                        }

                        return WebResponseContent.Instance.OK("ok", result);
                    }
                    else
                        return WebResponseContent.Instance.Error(_localizationService.GetString("no_files"));
                }
                else
                    return WebResponseContent.Instance.Error(_localizationService.GetString("no_files"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_FileService.GetDirectoryContents", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}

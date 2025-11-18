
using AutoMapper;
using Dm.util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Configuration;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Quotation_RecordService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_File_RecordsRepository _repositoryFileRecord;//访问数据库
        private readonly IBiz_Quotation_RecordRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private DbContext dbContext = DBServerProvider.DbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ISys_File_RecordsService _FileRecordsService;
        private readonly IBiz_Quotation_Record_ExcelRepository _repositoryQRExcel;

        [ActivatorUtilitiesConstructor]
        public Biz_Quotation_RecordService(
            IBiz_Quotation_RecordRepository dbRepository,
            ISys_File_RecordsRepository repositoryFileRecord,
            ILocalizationService localizationService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IBiz_Quotation_Record_ExcelRepository repositoryQRExcel,
            ISys_File_RecordsService file_RecordsService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            _repositoryFileRecord = repositoryFileRecord;
            _FileRecordsService = file_RecordsService;
            _configuration = configuration;
            _repositoryQRExcel = repositoryQRExcel;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 新增报价单记录
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Add(QuotationRecordAddDto dtoQuotationRecord, IFormFile file = null)
        {
            try
            {
                if (dtoQuotationRecord == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                if (file == null || file.Length == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                if(!CommonHelper.ChekcFileExt(file.FileName, ["xlsx", "xls"]))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_extension_invalid") + " .xlsx、.xls");
                }

                Biz_Quotation_Record biz_Project = _repository.Find(p => p.version == dtoQuotationRecord.version && p.qn_id == dtoQuotationRecord.qn_id && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();

                if (biz_Project != null)
                {
                    return WebResponseContent.Instance.Error($"{biz_Project.version}{_localizationService.GetString("existent")}");
                }

                Biz_Quotation_Record biz_Quotation_Record = _mapper.Map<Biz_Quotation_Record>(dtoQuotationRecord);
                biz_Quotation_Record.id = Guid.NewGuid();
                biz_Quotation_Record.delete_status = (int)SystemDataStatus.Valid;
                biz_Quotation_Record.create_id = UserContext.Current.UserId;
                biz_Quotation_Record.create_name = UserContext.Current.UserName;
                biz_Quotation_Record.create_date = DateTime.Now;

                biz_Quotation_Record.create_id_by_contract = dtoQuotationRecord.create_id_by_contract;
                biz_Quotation_Record.create_name_by_contract = dtoQuotationRecord.create_name_by_contract;
                biz_Quotation_Record.create_name_by_date = dtoQuotationRecord.create_name_by_date;
                biz_Quotation_Record.update_id_by_contract = dtoQuotationRecord.update_id_by_contract;
                biz_Quotation_Record.update_name_by_contract = dtoQuotationRecord.update_name_by_contract;
                biz_Quotation_Record.update_id_by_date = dtoQuotationRecord.update_id_by_date;

                //string strRelFolder = "";   //数据库配置
                //(bool res, string strExt, string strFileRelPath) = UploadFile(UploadFileCode.Quotation_Record_Documents, file);

                //读取文件配置地址
                var strFolderPath = _FileRecordsService.GetFileSaveFolder(UploadFileCode.Quotation_Record_Documents);

                //保存文件
                var saveFileResult = _FileRecordsService.SaveFileByPath(new List<IFormFile> { file }, strFolderPath.data as string);
                var fileInfo = saveFileResult.data as List<FileInfoDto>;
                if (fileInfo == null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                }

                //保存数据库
                var fileData = new Sys_File_Records
                {
                    id = Guid.NewGuid(),
                    master_id = biz_Quotation_Record.id,
                    file_name = fileInfo[0].file_name,
                    file_ext = fileInfo[0].file_ext,
                    file_path = fileInfo[0].file_relative_path,
                    file_code = UploadFileCode.Quotation_Record_Documents,
                    file_size = (int)file.Length,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now,
                    upload_status = (int)UploadStatus.Finish
                };
                _repositoryFileRecord.Add(fileData);
                _repository.Add(biz_Quotation_Record);

                //添加详情
                (bool resExcel, List<SheetDataResult> resultExcel) = AddExcelByImport(Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileInfo[0].file_relative_path), biz_Quotation_Record.id);

                if (resExcel)
                {
                    _repository.SaveChanges();
                    return WebResponseContent.Instance.OK("Ok", biz_Quotation_Record);
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("quotation_record_excel_failed"));
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_RecordService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除报价单记录
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                Biz_Quotation_Record biz_Quotation_Record = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (biz_Quotation_Record != null)
                {
                    biz_Quotation_Record.delete_status = (int)SystemDataStatus.Invalid;
                    biz_Quotation_Record.modify_id = UserContext.Current.UserId;
                    biz_Quotation_Record.modify_name = UserContext.Current.UserName;
                    biz_Quotation_Record.modify_date = DateTime.Now;
                    _repository.Update(biz_Quotation_Record, true);

                    return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_RecordService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改报价单记录
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Edit(QuotationRecordEditDto dtoQuotationRecord, IFormFile file = null)
        {
            try
            {
                if (dtoQuotationRecord.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                if (file == null || file.Length == 0)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                }
                if (!CommonHelper.ChekcFileExt(file.FileName, ["xlsx", "xls"]))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_extension_invalid") + " .xlsx、.xls");
                }

                Biz_Quotation_Record biz_Quotation_Record = _repository.Find(p => p.id == dtoQuotationRecord.id && p.qn_id == dtoQuotationRecord.qn_id).FirstOrDefault();
                if (biz_Quotation_Record != null)
                {
                    bool resExcel = true;
                    DateTime nowTime = DateTime.Now;
                    if (file != null)
                    {
                        //(bool result, string strExt, string strFileRelPath) = UploadFile(strRelFolder, file);
                        //if (!result) { return WebResponseContent.Instance.Error(biz_Quotation_Record.id + _localizationService.GetString("non_existent")); }

                        //读取文件配置地址
                        var strFolderPath = _FileRecordsService.GetFileSaveFolder(UploadFileCode.Quotation_Record_Documents);
                        //保存文件
                        List<IFormFile> fflFiles = new List<IFormFile>();
                        fflFiles.Add(file);
                        var saveFileResult = _FileRecordsService.SaveFileByPath(fflFiles, strFolderPath.data as string);

                        var fileInfo = saveFileResult.data as List<FileInfoDto>;
                        if (fileInfo == null)
                        {
                            return WebResponseContent.Instance.Error($"{_localizationService.GetString("failed_save_file")}");
                        }

                        //先删除旧数据(只更新delete_status字段的值)
                        var objFileRecord = _repositoryFileRecord.Find(a => a.master_id == biz_Quotation_Record.id && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                        foreach (var item in objFileRecord)
                        {
                            item.modify_id = UserContext.Current.UserId;
                            item.modify_name = UserContext.Current.UserName;
                            item.modify_date = nowTime;
                            item.delete_status = (int)SystemDataStatus.Invalid;
                        }
                        _repositoryFileRecord.UpdateRange(objFileRecord);

                        //（新增）保存数据库
                        var fileData = new Sys_File_Records
                        {
                            id = Guid.NewGuid(),
                            master_id = biz_Quotation_Record.id,
                            file_name = fileInfo[0].file_name,
                            file_ext = fileInfo[0].file_ext,
                            file_path = fileInfo[0].file_relative_path,
                            file_code = UploadFileCode.Quotation_Record_Documents,
                            file_size = (int)file.Length,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_id = UserContext.Current.UserId,
                            create_name = UserContext.Current.UserName,
                            create_date = nowTime,
                            upload_status = (int)UploadStatus.Finish
                        };
                        _repositoryFileRecord.AddAsync(fileData);

                        //添加详情
                        (resExcel, List<SheetDataResult> resultExcel) = AddExcelByImport(Path.Combine(AppSetting.FileSaveSettings.FolderPath, fileInfo[0].file_relative_path), biz_Quotation_Record.id);
                    }

                    //biz_Quotation_Record.delete_status = dtoQuotationRecord.delete_status;     //默认这个不支持修改
                    //biz_Quotation_Record.version = dtoQuotationRecord.version;                 //默认这个不支持修改
                    biz_Quotation_Record.amount = dtoQuotationRecord.amount;
                    biz_Quotation_Record.file_name = dtoQuotationRecord.file_name;
                    biz_Quotation_Record.qn_id = dtoQuotationRecord.qn_id;

                    biz_Quotation_Record.create_id_by_contract = dtoQuotationRecord.create_id_by_contract;
                    biz_Quotation_Record.create_name_by_contract = dtoQuotationRecord.create_name_by_contract;
                    biz_Quotation_Record.create_name_by_date = dtoQuotationRecord.create_name_by_date;
                    biz_Quotation_Record.update_id_by_contract = dtoQuotationRecord.update_id_by_contract;
                    biz_Quotation_Record.update_name_by_contract = dtoQuotationRecord.update_name_by_contract;
                    biz_Quotation_Record.update_id_by_date = dtoQuotationRecord.update_id_by_date;

                    biz_Quotation_Record.modify_id = UserContext.Current.UserId;
                    biz_Quotation_Record.modify_name = UserContext.Current.UserName;
                    biz_Quotation_Record.modify_date = nowTime;
                    var res = _repository.Update(biz_Quotation_Record);

                    if (resExcel)
                    {
                        _repository.SaveChanges();
                        return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    }
                    else
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("quotation_record_excel_failed"));
                    }
                }
                else return WebResponseContent.Instance.Error(biz_Quotation_Record.id + _localizationService.GetString("non_existent"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_RecordService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取报价单记录列表
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQuotationRecordList(PageInput<SearchQuotationRecordDto> dtoSearchInput)
        {
            PageGridData<QuotationRecordListDto> pageGridData = new PageGridData<QuotationRecordListDto>();
            try
            {
                var search = dtoSearchInput.search;
                string strWhere = string.Empty;

                #region 查询条件

                strWhere = " where qr.delete_status = 0 ";

                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.version))
                    {
                        strWhere += $" and qr.version like '%{search.version.Replace("'", "''")}%' ";
                    }
                    if (search.amount > 0)
                    {
                        strWhere += $" and qr.amount='{search.amount}' ";
                    }
                    if (!string.IsNullOrEmpty(search.file_name))
                    {
                        strWhere += $" and qr.file_name like '%{search.file_name.Replace("'", "''")}%' ";
                    }
                    if (!string.IsNullOrEmpty(search.qn_id.toString()))
                    {
                        strWhere += $" and qr.qn_id='{search.qn_id}' ";
                    }
                }

                #endregion

                // 构建包含多表连接的SQL查询，优化查询性能
                var sql = $@"select qr.*,qn.qn_no from Biz_Quotation_Record qr
                            left join Biz_Quotation qn on qr.qn_id=qn.id
                        {strWhere}
                        order by qr.create_date desc";

                var list = DBServerProvider.SqlDapper.QueryQueryable<QuotationRecordListDto>(sql, null);
                var result = list.GetPageResult(dtoSearchInput);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_RecordService.GetQuotationRecordList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据ID获取报价单详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetQuotationRecordById(Guid guid)
        {
            try
            {
                QuotationRecordListDto dtoQuotationRecord = new QuotationRecordListDto();
                var detail = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (detail != null)
                {
                    dtoQuotationRecord.id = detail.id;
                    dtoQuotationRecord.delete_status = detail.delete_status;
                    dtoQuotationRecord.create_date = detail.create_date;
                    dtoQuotationRecord.version = detail.version;
                    dtoQuotationRecord.amount = detail.amount;
                    dtoQuotationRecord.file_name = detail.file_name;
                    dtoQuotationRecord.qn_id = detail.qn_id;

                    var dbContext = DBServerProvider.DbContext;
                    var modelQuotation = dbContext.Set<Biz_Quotation>().Where(p => p.id == dtoQuotationRecord.qn_id).FirstOrDefault();
                    if (modelQuotation != null)
                    {
                        dtoQuotationRecord.qn_no = modelQuotation.qn_no;
                        dtoQuotationRecord.status_code = modelQuotation.status_code;
                    }
                }

                return WebResponseContent.Instance.OK("OK", dtoQuotationRecord);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Quotation_RecordService.GetQuotationRecordById", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #region  -- 报价单Excel详情操作 --

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <remarks>
        /// savechange在其它地方调用
        /// </remarks>
        private (bool, List<SheetDataResult>) AddExcelByImport(string filePath, Guid guidQuotationRecordId)
        {
            bool res = true;
            List<SheetDataResult> result = new List<SheetDataResult>();
            try
            {
                var columns = _configuration.GetSection("quotation_record_excel_columns").Get<List<string>>();
                var dicColumns = _configuration.GetSection("quotation_record_excel_columns_list").Get<Dictionary<string, List<string>>>();

                // 读取Excel文件中的所有sheet数据
                result = NPOIHelper.ReadAllSheetsWithSpecifiedColumns(filePath, columns, dicColumns);
                DateTime nowTime = DateTime.Now;
                int line_number = 0;
                foreach(var item in result)
                {
                    var dt = item.Data;
                    line_number = 1;
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        Biz_Quotation_Record_Excel excel = new Biz_Quotation_Record_Excel();
                        excel.id = Guid.NewGuid();
                        excel.delete_status = (int)SystemDataStatus.Valid;
                        excel.create_id = UserContext.Current.UserId;
                        excel.create_name = UserContext.Current.UserName;
                        excel.create_date = nowTime;
                        excel.quotation_record_id = guidQuotationRecordId;
                        excel.sheet_name = item.SheetName;
                        excel.line_number = line_number;

                        excel.item_code = row[columns[0]].ToString();
                        excel.item_description = row[columns[1]].ToString();
                        excel.unit = row[columns[2]].ToString();
                        excel.quantity = row[columns[3]].ToString();
                        excel.unit_rage = row[columns[4]].ToString();
                        excel.amount = row[columns[5]].ToString();

                        _repositoryQRExcel.Add(excel);
                        line_number++;
                    }
                }                
            }
            catch (Exception e)
            {
                res = false;
                Log4NetHelper.Error("Biz_Quotation_RecordService.AddExcelByImport", e);
            }
            return (res, result);
        }

        #endregion

        #region  -- 已弃用 --

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="strRelFolder">配置文件类型代码获取存储的相对路径</param>
        /// <param name="file">文件类型</param>
        private (bool, string, string) UploadFile(string strRelFolder, IFormFile file)
        {
            bool res = true;
            string strExt = string.Empty, strFileRelPath = string.Empty;
            try
            {
                if (file == null || file.Length == 0)
                {
                    //return WebResponseContent.Instance.Error(_localizationService.GetString("file_upload_null"));
                    return (false, strExt, strFileRelPath);
                }
                string strSaveFolder = AppSetting.FileSaveSettings.FolderPath;
                string strFileName = file.FileName;

                // 获取文件名、扩展、相对路径
                strExt = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                strFileRelPath = Path.Combine(strRelFolder, strFileName);  //保存数据库中的相对路径
                strSaveFolder = Path.Combine(strSaveFolder, strRelFolder);     //文件存放的文件夹Path

                //判断文件夹是否存在，不存在则创建
                if (!Directory.Exists(strSaveFolder))
                {
                    Directory.CreateDirectory(strSaveFolder);
                }

                string fullFilePath = Path.Combine(strSaveFolder, strFileName);
                // 检查文件是否已存在
                if (File.Exists(fullFilePath))
                {
                    // 如果存在，则在文件名后添加一个唯一标识符
                    strFileName = $"{strFileName}_{CommonHelper.GenerateRandomDigitString()}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{strExt}";
                    strFileRelPath = Path.Combine(strRelFolder, strFileName);
                }

                //创建文件
                using (var stream = new FileStream(Path.Combine(strSaveFolder, strFileName), FileMode.Create))
                {
                    //填充内容
                    //file.CopyToAsync(stream);
                    //同步代替异步，避免请求结束后文件流未关闭
                    file.CopyTo(stream);
                }
            }
            catch (Exception e)
            {
                res = false;
            }
            return (res, strExt, strFileRelPath);
        }

        #endregion
    }
}
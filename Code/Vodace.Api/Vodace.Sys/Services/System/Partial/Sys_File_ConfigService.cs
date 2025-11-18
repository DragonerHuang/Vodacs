
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Vodace.Sys.Services
{
    public partial class Sys_File_ConfigService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_File_ConfigRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_ConfigRepository _configRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_File_ConfigService(
            ISys_File_ConfigRepository dbRepository,
            IMapper mapper,
            ILocalizationService localizationService,
            IHttpContextAccessor httpContextAccessor,
            ISys_ConfigRepository configRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            _configRepository = configRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        WebResponseContent webResponse = new WebResponseContent();

        /// <summary>
        /// 新增文件配置
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Add(AddFileConfigDto dtoFileConfig)
        {
            try
            {
                if (dtoFileConfig == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                if (string.IsNullOrEmpty(dtoFileConfig.file_code))
                    return WebResponseContent.Instance.Error($"{dtoFileConfig.file_code} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                if (string.IsNullOrEmpty(dtoFileConfig.folder_path))
                    return WebResponseContent.Instance.Error($"{dtoFileConfig.folder_path} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                dtoFileConfig.file_code = dtoFileConfig.file_code.Replace("\\\\", "\\");
                dtoFileConfig.folder_path = dtoFileConfig.folder_path.Replace("\\\\", "\\");

                Sys_File_Config model = _repository.Find(p => p.file_code == dtoFileConfig.file_code && p.folder_path == dtoFileConfig.folder_path && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();

                if (model != null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("file_code")}:{dtoFileConfig.file_code},{_localizationService.GetString("folder_path")}:{dtoFileConfig.folder_path} {_localizationService.GetString("already_exists")}");
                }

                Sys_File_Config sys_File_Config = _mapper.Map<Sys_File_Config>(dtoFileConfig);
                sys_File_Config.id = Guid.NewGuid();
                sys_File_Config.delete_status = (int)SystemDataStatus.Valid;
                sys_File_Config.create_id = UserContext.Current.UserId;
                sys_File_Config.create_name = UserContext.Current.UserName;
                sys_File_Config.create_date = DateTime.Now;
                _repository.Add(sys_File_Config, true);
                return WebResponseContent.Instance.OK("Ok", sys_File_Config);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_File_ConfigService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除文件配置
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                Sys_File_Config sys_File_Config = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (sys_File_Config != null)
                {
                    sys_File_Config.delete_status = (int)SystemDataStatus.Invalid;
                    sys_File_Config.modify_id = UserContext.Current.UserId;
                    sys_File_Config.modify_name = UserContext.Current.UserName;
                    sys_File_Config.modify_date = DateTime.Now;
                    _repository.Update(sys_File_Config, true);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_File_ConfigService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改文件配置
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Edit(EditFileConfigDto dtoFileConfig)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoFileConfig.id.ToString()) || dtoFileConfig.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));

                Sys_File_Config model = _repository.Find(p => p.file_code == dtoFileConfig.file_code && p.folder_path == dtoFileConfig.folder_path && p.id != dtoFileConfig.id && p.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();

                if (model != null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("file_code")}:{dtoFileConfig.file_code},{_localizationService.GetString("folder_path")}:{dtoFileConfig.folder_path} {_localizationService.GetString("already_exists")}");
                }

                if (string.IsNullOrEmpty(dtoFileConfig.file_code))
                    return WebResponseContent.Instance.Error($"{dtoFileConfig.file_code} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                if (string.IsNullOrEmpty(dtoFileConfig.folder_path))
                    return WebResponseContent.Instance.Error($"{dtoFileConfig.folder_path} {_localizationService.GetString("message")} {_localizationService.GetString("connot_be_empty")}");

                dtoFileConfig.file_code = dtoFileConfig.file_code.Replace("\\\\", "\\");
                dtoFileConfig.folder_path = dtoFileConfig.folder_path.Replace("\\\\", "\\");

                Sys_File_Config sys_File_Config = _repository.Find(p => p.id == dtoFileConfig.id).FirstOrDefault();
                if (sys_File_Config != null)
                {
                    sys_File_Config.file_code = dtoFileConfig.file_code;
                    sys_File_Config.folder_path = dtoFileConfig.folder_path;
                    sys_File_Config.remark = dtoFileConfig.remark;

                    sys_File_Config.modify_id = UserContext.Current.UserId;
                    sys_File_Config.modify_name = UserContext.Current.UserName;
                    sys_File_Config.modify_date = DateTime.Now;
                    var res = _repository.Update(sys_File_Config, true);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"), sys_File_Config);
                }
                else return WebResponseContent.Instance.Error(sys_File_Config.id + _localizationService.GetString("non_existent"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_File_ConfigService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取文件配置列表
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetFileConfigList(PageInput<AddFileConfigDto> dtoSearchInput)
        {
            PageGridData<FileConfigDto> pageGridData = new PageGridData<FileConfigDto>();
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == (int)SystemDataStatus.Valid
            && (string.IsNullOrEmpty(dtoSearchInput.search.file_code) ? true : d.file_code == dtoSearchInput.search.file_code)
            && (string.IsNullOrEmpty(dtoSearchInput.search.folder_path) ? true : d.folder_path == dtoSearchInput.search.folder_path)
            && (string.IsNullOrEmpty(dtoSearchInput.search.remark) ? true : d.remark == dtoSearchInput.search.remark)
            ).Select(d => new FileConfigDto
            {
                id = d.id,
                file_code = d.file_code,
                folder_path = d.folder_path,
                remark = d.remark,
                create_name = d.create_name,
                create_date = d.create_date,
                modify_date = d.modify_date,
                modify_name = d.modify_name
            });

            var result = await lstData.GetPageResultAsync(dtoSearchInput);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        /// <summary>
        /// 获取文件配置ID及名称
        /// </summary>
        /// <returns></returns>
        /// <remarks>获取delete_status=0(未删除数据)</remarks>
        public async Task<WebResponseContent> GetFileConfigAllName()
        {
            try
            {
                var list = _repository.Find(p => p.delete_status == (int)SystemDataStatus.Valid)
                    .Select(x => new { x.id, x.file_code, x.folder_path }).ToList();
                return WebResponseContent.Instance.OK("OK", list);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_File_ConfigService.GetFileConfigAllName", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据ID获取文件配置信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetFileConfigById(Guid guid)
        {
            try
            {
                return WebResponseContent.Instance.OK("OK", _repository.Find(p => p.id == guid).FirstOrDefault());
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_File_ConfigService.GetFileConfigById", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据年份 获取项目年度文件夹
        /// </summary>
        /// <returns></returns>
        public WebResponseContent GetMainProFolderName(DateTime dateTime)
        {
            try
            {
                var config = _configRepository.FindFirst(p => p.config_key == "HKFiscalYear" && p.config_type == 2 && p.delete_status == (int)SystemDataStatus.Valid);
                if (config == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_config_null"));
                }

                return WebResponseContent.Instance.OK("ok", DoProjectFolderName(config, dateTime));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据年份 获取项目年度文件夹(异步)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetMainProFolderNameAsync(DateTime dateTime)
        {
            try
            {
                var config = await _configRepository
                    .FindFirstAsync(p => p.config_key == "HKFiscalYear" && p.config_type == 2 && p.delete_status == (int)SystemDataStatus.Valid);
                if (config == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("file_config_null"));
                }

                return WebResponseContent.Instance.OK("ok", DoProjectFolderName(config, dateTime));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 根据年份 获取项目年度文件夹
        /// </summary>
        /// <param name="fileConfig"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private string DoProjectFolderName(Sys_Config fileConfig, DateTime dateTime)
        {
            var year = dateTime.Year;
            var strRootFolder = "Project ";
            if (fileConfig.config_value == "1")
            {
                DateTime aprilFirst = new DateTime(year, 4, 1);
                strRootFolder = dateTime.Date < aprilFirst.Date ? $"Project {year - 1}-{year}" : $"Project {year}-{year + 1}";
            }
            else
            {
                strRootFolder = $"Project {year}-{year + 1}";
            }

            return strRootFolder;
        }

        public bool AddCofig(string file_code, string folder_path)
        {
            try
            {
                var old = _repository.Find(p => p.file_code == file_code).FirstOrDefault();
                if (old != null) return true;
                else
                {
                    Sys_File_Config sys_File_Config = new Sys_File_Config
                    {
                        id = Guid.NewGuid(),
                        file_code = file_code,
                        folder_path = folder_path,
                        delete_status = (int)SystemDataStatus.Valid,
                        create_date = DateTime.Now,
                        create_name = UserContext.Current.UserName,
                        create_id = UserContext.Current.UserId,
                        remark = "系统创建配置"
                    };
                    _repository.Add(sys_File_Config);
                    _repository.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return false;
            }
        }
    }
}

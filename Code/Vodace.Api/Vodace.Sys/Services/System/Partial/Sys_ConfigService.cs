
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Vodace.Sys.Services
{
    public partial class Sys_ConfigService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_ConfigRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_ConfigService(
            ISys_ConfigRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService,
            IMapper mapper)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> GetListByPage(PageInput<ConfigQuery> query)
        {
            PageGridData<ConfigListDto> pageGridData = new PageGridData<ConfigListDto>();
            var queryPam = query.search;
            var lstData = _repository.FindAsIQueryable(d => d.delete_status == 0 &&
            (queryPam.config_type == -1 ? true : d.config_type == queryPam.config_type) &&
            (string.IsNullOrEmpty(queryPam.config_key) ? true : d.config_key.Contains(queryPam.config_key)))
            .Select(d => new ConfigListDto
            {
                id = d.id,
                config_type = d.config_type,
                config_key = d.config_key,
                config_value = d.config_value,
                create_date = d.create_date,
                create_name = d.create_name,
                modify_date = d.modify_date,
                modify_name = d.modify_name,
                remark = d.remark
            });
            var result = await lstData.GetPageResultAsync(query);
            return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
        }

        public WebResponseContent Add(ConfigAddDto config)
        {
            try
            {
                if (config == null) return WebResponseContent.Instance.Error(_localizationService.GetString("connot_be_empty！"));
                var checkKey = _repository.Exists(d => d.delete_status == (int)SystemDataStatus.Valid && d.config_key == config.config_key && d.config_type == config.config_type);
                if (checkKey) return WebResponseContent.Instance.Error(_localizationService.GetString("config_key_exist！"));
                Sys_Config sys_config = _mapper.Map<Sys_Config>(config);
                sys_config.delete_status = (int)SystemDataStatus.Valid;
                sys_config.create_date = DateTime.Now;
                sys_config.create_name = UserContext.Current.UserName;
                sys_config.id = Guid.NewGuid();
                _repository.Add(sys_config, true);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), config);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent Update(ConfigEditDto config) 
        {
            try
            {
                if (config.id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var entData = _repository.Find(d => d.id == config.id).FirstOrDefault();
                if (entData != null)
                {
                    entData.config_type = config.config_type;
                    entData.config_key = config.config_key;
                    entData.config_value = config.config_value;
                    entData.modify_date = DateTime.Now;
                    entData.modify_name = UserContext.Current.UserName;

                    var res = _repository.Update(entData, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), entData.id);
                    else return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public WebResponseContent Delete(Guid id) 
        {
            try
            {
                if (id == Guid.Empty) return WebResponseContent.Instance.Error("id_null");
                var entModel = _repository.FindAsIQueryable(x => x.id == id).FirstOrDefault();
                if (UserContext.Current.IsSuperAdmin)
                {
                    entModel.delete_status = (int)SystemDataStatus.Invalid;
                    entModel.modify_name = UserContext.Current.UserName;
                    entModel.modify_date = DateTime.Now;
                    var res = _repository.Update(entModel, true);
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), res);
                }
                else
                {
                    //权限不足
                    return WebResponseContent.Instance.Error(_localizationService.GetString("insufficient_permissions"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> GetConfigByTypeAsync(int configType, string congfigKey)
        {
            try
            {
                var data = await _repository.FindFirstAsync(p => p.config_type == configType && p.config_key == congfigKey);
                if (data == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("config_null"));
                }

                var config = new ConfigListDto
                {
                    id = data.id,
                    config_type = data.config_type,
                    config_key = data.config_key,
                    config_value = data.config_value,
                    create_date = data.create_date,
                    create_name = data.create_name,
                    modify_date = data.modify_date,
                    modify_name = data.modify_name,
                    remark = data.remark
                };

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), config);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed"));
            }
        }
    }
}

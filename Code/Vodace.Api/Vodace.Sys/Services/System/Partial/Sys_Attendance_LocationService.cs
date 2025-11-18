
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

namespace Vodace.Sys.Services
{
    public partial class Sys_Attendance_LocationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Attendance_LocationRepository _repository;// 访问数据库

        private readonly ILocalizationService _localizationService;     // 国际化
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Sys_Attendance_LocationService(
            ISys_Attendance_LocationRepository dbRepository,
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

        /// <summary>
        /// 获取数据列表（分页）
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDataListAsync(PageInput<LocationSearchDto> dtoSearchInput)
        {
            try
            {
                var where = _repository.WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid).AsNoTracking();
                if (!string.IsNullOrEmpty(dtoSearchInput.search.location_name))
                {
                    where = where.Where(dto => dto.location_name.Contains(dtoSearchInput.search.location_name));
                }

                if (string.IsNullOrEmpty(dtoSearchInput.sort_field))
                {
                    dtoSearchInput.sort_field = "create_date";
                    dtoSearchInput.sort_type = "asc";
                }

                var result = await where.GetPageResultAsync(dtoSearchInput);

                var data = new PageData<LocationDataDto>
                {
                    total = result.total,
                    data = new List<LocationDataDto>()
                };
                foreach (var item in result.data)
                {
                    data.data.Add(new LocationDataDto
                    {
                        id = item.id,
                        location_name = item.location_name,
                        create_id = item.create_id,
                        create_name = item.create_name,
                        create_date = item.create_date,
                        modify_id = item.modify_id,
                        modify_name = item.modify_name,
                        modify_date = item.modify_date
                    });
                }


                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), data);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取数据列表（不分页）
        /// </summary>
        /// <param name="dtoSearchInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetDataListAsync(LocationSearchDto dtoSearchInput)
        {
            try
            {
                var where = _repository.WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid).AsNoTracking();
                if (dtoSearchInput != null && !string.IsNullOrEmpty(dtoSearchInput.location_name))
                {
                    where = where.Where(dto => dto.location_name.Contains(dtoSearchInput.location_name));
                }
                var result = await where.OrderBy(p => p.create_date).Select(item => new LocationDataDto
                {
                    id = item.id,
                    location_name = item.location_name,
                    create_id = item.create_id,
                    create_name = item.create_name,
                    create_date = item.create_date,
                    modify_id = item.modify_id,
                    modify_name = item.modify_name,
                    modify_date = item.modify_date

                }).ToListAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="dtoInput"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> AddData(LocationInputDto dtoInput)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoInput.location_name))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("location_name_null"));
                }
                var data = new Sys_Attendance_Location
                {
                    id = Guid.NewGuid(),
                    location_name = dtoInput.location_name,
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserInfo.User_Id,
                    create_name = UserContext.Current.UserInfo.UserName,
                    create_date = DateTime.Now,
                };

                await _repository.AddAsync(data);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="dtoEdit"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> EditData(LocationEditDto dtoEdit)
        {
            try
            {
                if (string.IsNullOrEmpty(dtoEdit.location_name))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("location_name_null"));
                }
                var data = _repository.FindFirst(p => p.id == dtoEdit.id);
                if (data == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("location_null"));
                }
                data.location_name = dtoEdit.location_name;
                data.modify_id = UserContext.Current.UserInfo.User_Id;
                data.modify_name = UserContext.Current.UserInfo.UserName;
                data.modify_date = DateTime.Now;

                _repository.Update(data);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除数据（软删）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteData(Guid id)
        {
            try
            {
                var data = _repository.FindFirst(p => p.id == id);
                if (data == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("location_null"));
                }
                data.delete_status = (int)SystemDataStatus.Invalid;
                data.modify_id = UserContext.Current.UserInfo.User_Id;
                data.modify_name = UserContext.Current.UserInfo.UserName;
                data.modify_date = DateTime.Now;

                _repository.Update(data);
                await _repository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
  
}

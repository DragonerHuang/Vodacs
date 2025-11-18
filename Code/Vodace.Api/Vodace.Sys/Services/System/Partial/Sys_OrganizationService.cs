
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Collections.Generic;
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
using static Dapper.SqlMapper;

namespace Vodace.Sys.Services
{
    public partial class Sys_OrganizationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_OrganizationRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;

        [ActivatorUtilitiesConstructor]
        public Sys_OrganizationService(
            ISys_OrganizationRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public WebResponseContent Add(OrganizationDto dto)
        {
            try
            {
                Sys_Organization model = new Sys_Organization();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;
                model.enable = 1;

                if (string.IsNullOrEmpty(dto.name_eng) && string.IsNullOrEmpty(dto.name_cht) && string.IsNullOrEmpty(dto.name_sho) && string.IsNullOrEmpty(dto.name_ali))
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "name"));

                var exist = _repository.Find(a => a.name_eng == dto.name_eng && a.name_cht == dto.name_cht && a.delete_status == (int)SystemDataStatus.Valid).ToList();
                if (exist.Count > 0)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_exist", dto.name_eng));

                model.master_id = dto.master_id;
                model.name_eng = dto.name_eng;
                model.name_cht = dto.name_cht;
                model.name_sho = dto.name_sho;
                model.name_ali = dto.name_ali;
                model.remark = dto.remark;

                _repository.Add(model, true);
                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_OrganizationService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                var model = _repository.Find(p => p.id == guid).FirstOrDefault();
                if (model != null)
                {
                    //判断是否存在子级
                    var list = _repository.Find(p => p.master_id == model.id && p.delete_status == (int)SystemDataStatus.Valid).ToList();
                    if (list.Count > 0)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("sub_level_not_allowed"));

                    model.delete_status = (int)SystemDataStatus.Invalid;
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;
                    var res = _repository.Update(model, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("delete") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("delete") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{guid}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_OrganizationService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(OrganizationEditDto dto)
        {
            try
            {
                var model = repository.Find(p => p.id == dto.id).FirstOrDefault();
                if (model != null)
                {
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    if (string.IsNullOrEmpty(dto.name_eng) && string.IsNullOrEmpty(dto.name_cht) && string.IsNullOrEmpty(dto.name_sho) && string.IsNullOrEmpty(dto.name_ali))
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "name"));

                    var exist = _repository.Find(a => a.name_eng == dto.name_eng && a.name_cht == dto.name_cht && a.delete_status == (int)SystemDataStatus.Valid && a.id != dto.id).ToList();
                    if (exist.Count > 0)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_exist", dto.name_eng));

                    model.enable = dto.enable;
                    model.master_id = dto.master_id;
                    model.name_eng = dto.name_eng;
                    model.name_cht = dto.name_cht;
                    model.name_sho = dto.name_sho;
                    model.name_ali = dto.name_ali;
                    model.remark = dto.remark;

                    var res = _repository.Update(model, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{dto.id}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_OrganizationService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Enable(OrganizationEnableDto dto)
        {
            try
            {
                var model = repository.Find(p => p.id == dto.id).FirstOrDefault();
                if (model != null)
                {
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    model.enable = dto.enable;
                    var res = _repository.Update(model, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{dto.id}{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_OrganizationService.Enable", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetOrganizationList(PageInput<OrganizationEditDto> dto)
        {
            try
            {
                var list = _repository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                var query = from r in list
                            join t in list
                            on r.master_id equals t.id into master
                            from lt in master.DefaultIfEmpty()
                            select new DepartmentListDto()
                            {
                                id = r.id,

                                master_id = r.master_id,
                                name_eng = r.name_eng,
                                name_cht = r.name_cht,
                                name_sho = r.name_sho,
                                name_ali = r.name_ali,
                                enable = r.enable,
                                remark = r.remark,

                                master_name_eng = lt.name_eng,
                                master_name_cht = lt.name_cht,
                                master_name_sho = lt.name_sho,
                                master_name_ali = lt.name_ali,

                                create_id = r.create_id,
                                create_name = r.create_name,
                                create_date = r.create_date,
                                modify_id = r.modify_id,
                                modify_name = r.modify_name,
                                modify_date = r.modify_date,
                            };

                // 获取查询条件
                var searchDto = dto.search;
                // 添加查询条件过滤
                if (searchDto != null)
                {
                    query = query.WhereIF(searchDto.master_id.HasValue, p => p.master_id == searchDto.master_id);
                    query = query.WhereIF(searchDto.id.HasValue, p => p.id == searchDto.id);
                    query = query.WhereIF(searchDto.enable > -1, p => p.enable == searchDto.enable);

                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.remark), p => p.remark.Contains(searchDto.remark));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.name_ali), p => p.name_ali.Contains(searchDto.name_ali));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.name_sho), p => p.name_sho.Contains(searchDto.name_sho));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.name_cht), p => p.name_cht.Contains(searchDto.name_cht));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.name_eng), p => p.name_cht.Contains(searchDto.name_eng));
                }

                if (!string.IsNullOrEmpty(dto.sort_field))
                    query = query.OrderByDynamic(dto.sort_field, dto.sort_type);
                else
                    query = query.OrderByDescending(x => x.create_date);

                var sql = query.ToQueryString();

                // 执行分页查询（使用项目提供的扩展方法）
                var result = await query.GetPageResultAsync(dto);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_OrganizationService.GetDepartmentList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}


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

namespace Vodace.Sys.Services
{
    public partial class Sys_User_RelationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_User_RelationRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly ISys_User_NewRepository _userRepository;//访问数据库
        private readonly ISys_Work_TypeRepository _workTypeRepository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public Sys_User_RelationService(
            ISys_User_RelationRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
,
            ILocalizationService localizationService,
            ISys_User_NewRepository userRepository,
            ISys_Work_TypeRepository workTypeRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _userRepository = userRepository;
            _workTypeRepository = workTypeRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public WebResponseContent Add(SysUserRelationDto dto)
        {
            try
            {
                Sys_User_Relation model = new Sys_User_Relation();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                model.relation_type = dto.relation_type;
                model.user_register_Id = dto.user_register_Id;
                model.relation_id = dto.relation_id;
                model.day_salary = dto.day_salary;
                model.night_salary = dto.night_salary;
                model.remark = dto.remark;

                _repository.Add(model, true);

                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_User_RelationService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                var model = repository.Find(p => p.id == guid).FirstOrDefault();
                if (model != null)
                {
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
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_User_RelationService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(SysUserRelationDto dto)
        {
            try
            {
                var model = repository.Find(p => p.id == dto.id).FirstOrDefault();
                if (model != null)
                {
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    model.relation_type = dto.relation_type;
                    model.user_register_Id = dto.user_register_Id;
                    model.relation_id = dto.relation_id;
                    model.day_salary = dto.day_salary;
                    model.night_salary = dto.night_salary;
                    model.remark = dto.remark;

                    var res = _repository.Update(model, true);

                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
                }
                else
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_User_RelationService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetSysUserRelationList(PageInput<SysUserRelationSearchDto> dto)
        {
            try
            {
                var lstRelation = _repository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstUser = _userRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);
                var lstWT = _workTypeRepository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);

                var query = from r in lstRelation
                            join u in lstUser
                            on r.user_register_Id equals u.user_register_id into ru
                            from lt in ru.DefaultIfEmpty()

                            join t in lstWT
                            on r.relation_id equals t.id into rw
                            from tr in rw.DefaultIfEmpty()

                            select new
                            {
                                r.id,
                                r.remark,
                                r.user_register_Id,
                                r.relation_id,
                                r.relation_type,
                                r.day_salary,
                                r.night_salary,
                                r.create_id,
                                r.create_name,
                                r.create_date,
                                r.modify_id,
                                r.modify_name,
                                r.modify_date,

                                lt.user_name_eng,
                                lt.user_true_name,
                                lt.user_name,

                                tr.type_name,
                            };

                // 获取查询条件
                var searchDto = dto.search;
                // 添加查询条件过滤
                if (searchDto != null)
                {
                    query = query.WhereIF(searchDto.user_register_Id.HasValue, p => p.user_register_Id == searchDto.user_register_Id);
                    query = query.WhereIF(searchDto.relation_id.HasValue, p => p.relation_id == searchDto.relation_id);
                    query = query.WhereIF(searchDto.id.HasValue, p => p.id == searchDto.id);

                    query = query.WhereIF(searchDto.relation_type > -1, p => p.relation_type == searchDto.relation_type);
                    query = query.WhereIF(searchDto.day_salary > -1, p => p.day_salary == searchDto.day_salary);
                    query = query.WhereIF(searchDto.night_salary > -1, p => p.night_salary == searchDto.night_salary);

                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.remark), p => p.remark.Contains(searchDto.remark));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.user_name_eng), p => p.user_name_eng.Contains(searchDto.user_name_eng));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.user_true_name), p => p.user_true_name.Contains(searchDto.user_true_name));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.user_name), p => p.user_name.Contains(searchDto.user_name));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.type_name), p => p.type_name.Contains(searchDto.type_name));
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
                Log4NetHelper.Error("Sys_User_RelationService.GetSysUserRelationList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}

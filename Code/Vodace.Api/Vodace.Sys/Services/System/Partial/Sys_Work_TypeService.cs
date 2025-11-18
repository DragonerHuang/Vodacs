
using Dm.util;
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
using Vodace.Entity.DomainModels.Common;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Sys_Work_TypeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Work_TypeRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;

        [ActivatorUtilitiesConstructor]
        public Sys_Work_TypeService(
            ISys_Work_TypeRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ILocalizationService localizationService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }


        public WebResponseContent Add(WorkTypeDto dto)
        {
            try
            {
                Sys_Work_Type model = new Sys_Work_Type();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                model.master_id = dto.master_id;
                model.type_name = dto.type_name;
                model.day_salary = dto.day_salary;
                model.night_salary = dto.night_salary;
                model.remark = dto.remark;

                _repository.Add(model, true);

                return WebResponseContent.Instance.OK("Ok", model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Work_TypeService.Add", e);
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
                Log4NetHelper.Error("Sys_Work_TypeService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(WorkTypeDto dto)
        {
            try
            {
                var model = repository.Find(p => p.id == dto.id).FirstOrDefault();
                if (model != null)
                {
                    //判断是否存在子级
                    var list = _repository.Find(p => p.master_id == model.id && p.delete_status == (int)SystemDataStatus.Valid).ToList();
                    if(list.Count > 0)
                        return WebResponseContent.Instance.Error(_localizationService.GetString("sub_level_not_allowed"));

                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    model.master_id = dto.master_id;
                    model.type_name = dto.type_name;
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
                Log4NetHelper.Error("Sys_Work_TypeService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public async Task<WebResponseContent> GetWorkTypeList(PageInput<WorkTypeDto> dto)
        {
            try
            {
                var list = _repository.FindAsIQueryable(p => p.delete_status == (int)SystemDataStatus.Valid);

                var query = from r in list
                            join t in list
                            on r.master_id equals t.id into master
                            from lt in master.DefaultIfEmpty()

                            select new 
                            {
                                r.id,
                                r.master_id,
                                r.remark,

                                r.type_name,
                                master_type_name = lt != null ? lt.type_name : string.Empty,
                                r.day_salary,
                                r.night_salary,

                                r.create_id,
                                r.create_name,
                                r.create_date,
                                r.modify_id,
                                r.modify_name,
                                r.modify_date,
                            };

                // 获取查询条件
                var searchDto = dto.search;
                // 添加查询条件过滤
                if (searchDto != null)
                {
                    query = query.WhereIF(searchDto.master_id.HasValue, p => p.master_id == searchDto.master_id);
                    query = query.WhereIF(searchDto.id.HasValue, p => p.id == searchDto.id);

                    query = query.WhereIF(searchDto.day_salary > -1, p => p.day_salary == searchDto.day_salary);
                    query = query.WhereIF(searchDto.night_salary > -1, p => p.night_salary == searchDto.night_salary);

                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.remark), p => p.remark.Contains(searchDto.remark));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.type_name), p => p.type_name.Contains(searchDto.type_name));
                    query = query.WhereIF(!string.IsNullOrEmpty(searchDto.master_type_name), p => p.master_type_name.Contains(searchDto.master_type_name));
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
                Log4NetHelper.Error("Sys_Work_TypeService.GetWorkTypeList", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 获取工种类型
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetWorkTypeAllName()
        {
            try
            {
                var list = await _repository
                    .WhereIF(true, p => p.delete_status == (int)SystemDataStatus.Valid)
                    .Select(x => new { x.id, name = x.type_name }).ToListAsync();
                return WebResponseContent.Instance.OK("OK", list);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Work_TypeService.GetWorkTypeAllName", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        public async Task<WebResponseContent> GetDataTree()
        {
            try
            {
                var list = await _repository.FindAsync(d => d.delete_status == (int)SystemDataStatus.Valid);
                var parent = list.Where(d => d.master_id == null);
                var children = list.Where(d => d.master_id != null);
                List<TreeNode> lst = new List<TreeNode>();
                foreach (var item in parent)
                {
                    TreeNode treeNode = new TreeNode();
                    treeNode.value = item.type_name;
                    treeNode.lable = item.id.ToString();
                    foreach (var child in children.Where(d => d.master_id == item.id))
                    {
                        Children children1 = new Children();
                        children1.value = child.type_name;
                        children1.lable = child.id.ToString();
                        treeNode.children.Add(children1);
                    }
                    lst.Add(treeNode);
                }
                //var json = treeNode.ToJson();
                return WebResponseContent.Instance.OK("Ok", lst);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Sys_Work_TypeService.GetDataTree", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}

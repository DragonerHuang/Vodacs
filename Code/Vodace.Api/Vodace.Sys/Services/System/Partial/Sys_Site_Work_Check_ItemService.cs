
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;
using System.Linq;
using Vodace.Core.Utilities;
using System.Linq.Expressions;
using Vodace.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Vodace.Sys.IRepositories;
using Vodace.Core.ManageUser;
using Vodace.Core.Enums;
using Vodace.Core.Localization;
using System.Threading.Tasks;
using Vodace.Entity;
using Vodace.Core.Utilities.Log4Net;
using System;

namespace Vodace.Sys.Services
{
    public partial class Sys_Site_Work_Check_ItemService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Site_Work_Check_ItemRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;

        [ActivatorUtilitiesConstructor]
        public Sys_Site_Work_Check_ItemService(
            ISys_Site_Work_Check_ItemRepository dbRepository,
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

        /// <summary>
        /// 分页查询：按 global_code 前缀匹配；默认排序 level→item_code→order_no（升序）
        /// </summary>
        /// <param name="input">查询与分页参数</param>
        /// <returns>分页结果</returns>
        public async Task<WebResponseContent> GetPageListAsync(PageInput<SiteWorkCheckItemSearchDto> input)
        {
            try
            {
                var valid = (int)SystemDataStatus.Valid;
                var search = input.search;
                var query = _repository
                    .FindAsIQueryable(p => p.delete_status == valid)
                    .WhereIF(search != null && !string.IsNullOrEmpty(search.global_code), p => p.global_code.StartsWith(search.global_code))
                    .WhereIF(search != null && search.level.HasValue, p => p.level == search.level.Value)
                    .WhereIF(search != null && search.root_id.HasValue, p => p.root_id == search.root_id.Value)
                    .WhereIF(search != null && search.master_id.HasValue, p => p.master_id == search.master_id.Value)
                    .WhereIF(search != null && !string.IsNullOrEmpty(search.name), p => p.name_cht.Contains(search.name) || p.name_eng.Contains(search.name))
                    .AsNoTracking();

                // 默认排序：level→item_code→order_no
                query = query
                    .OrderBy(p => p.level)
                    .ThenBy(p => p.item_code)
                    .ThenBy(p => p.order_no);

                // 投影为查询输出DTO
                var dtoQuery = query.Select(p => new SiteWorkCheckItemDto
                {
                    id = p.id,
                    level = p.level,
                    item_code = p.item_code,
                    root_id = p.root_id,
                    master_id = p.master_id,
                    global_code = p.global_code,
                    name_cht = p.name_cht,
                    name_eng = p.name_eng,
                    order_no = p.order_no,
                    enable = p.enable
                });

                var result = await dtoQuery.GetPageResultAsync(input);
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 新增：校验 global_code + type_id 唯一性；默认 type_id/is_qly/is_others 为 0
        /// </summary>
        /// <param name="dto">新增输入DTO</param>
        /// <returns>操作结果</returns>
        public async Task<WebResponseContent> AddAsync(SiteWorkCheckItemAddDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("connot_be_empty"));
                }
                   
                if (string.IsNullOrEmpty(dto.global_code))
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_null", "global_code"));
                }
                    
                var valid = (int)SystemDataStatus.Valid;
                var exist = await _repository
                    .FindAsIQueryable(p => p.global_code == dto.global_code && 
                                           p.type_id == dto.type_id &&
                                           p.delete_status == valid)
                    .AnyAsync();

                if (exist)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_exist", dto.global_code));
                }
               

                var model = new Sys_Site_Work_Check_Item
                {
                    id = Guid.NewGuid(),
                    delete_status = (int)SystemDataStatus.Valid,
                    create_id = UserContext.Current.UserId,
                    create_name = UserContext.Current.UserName,
                    create_date = DateTime.Now,
                    enable = dto.enable.HasValue ? dto.enable : (byte)1,
                    level = dto.level,
                    root_id = dto.root_id,
                    master_id = dto.master_id,
                    global_code = dto.global_code,
                    name_cht = dto.name_cht,
                    name_eng = dto.name_eng,
                    order_no = dto.order_no,
                    type_id = dto.type_id ?? 0,
                    is_qly = 0,
                    is_others = 0,
                };

                _repository.Add(model);

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
        /// 修改：item_code；global_code+type_id 唯一校验；缺省值按新增规则处理
        /// </summary>
        /// <param name="dto">编辑输入DTO</param>
        /// <returns>操作结果</returns>
        public async Task<WebResponseContent> EditAsync(SiteWorkCheckItemEditDto dto)
        {
            try
            {
                if (dto == null || dto.id == Guid.Empty)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("connot_be_empty"));
                }
                    
                var valid = (int)SystemDataStatus.Valid;
                var model = await _repository
                    .FindAsIQueryable(p => p.id == dto.id && p.delete_status == valid)
                    .FirstOrDefaultAsync();
                if (model == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent"));
                }
                
                if (!string.IsNullOrEmpty(dto.global_code) && (dto.global_code != model.global_code || dto.type_id != model.type_id))
                {
                    var exist = await _repository
                        .FindAsIQueryable(p => p.global_code == dto.global_code && 
                                               p.type_id == dto.type_id && 
                                               p.id != dto.id && 
                                               p.delete_status == valid)
                        .AnyAsync();
                    if (exist)
                    {
                        return WebResponseContent.Instance.Error(_localizationService.GetString("key_type_exist", dto.global_code));
                    }
                    model.global_code = dto.global_code;
                    model.type_id = dto.type_id;
                }

                model.level = dto.level;
                model.root_id = dto.root_id;
                model.master_id = dto.master_id;
                model.is_qly =  0;
                model.is_others =  0;
                model.name_cht = dto.name_cht;
                model.name_eng = dto.name_eng;
                model.order_no = dto.order_no;
                model.enable = dto.enable ?? 1;

                model.modify_id = UserContext.Current.UserId;
                model.modify_name = UserContext.Current.UserName;
                model.modify_date = DateTime.Now;

                _repository.Update(model, x => new
                {
                    x.level,
                    x.root_id,
                    x.master_id,
                    x.global_code,
                    x.type_id,
                    x.is_qly,
                    x.is_others,
                    x.name_cht,
                    x.name_eng,
                    x.order_no,
                    x.remark,
                    x.enable,
                    x.modify_id,
                    x.modify_name,
                    x.modify_date
                });

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
        /// 删除：逻辑删除，设置 delete_status=Invalid
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作结果</returns>
        public async Task<WebResponseContent> DelAsync(Guid id)
        {
            try
            {
                var model = await _repository
                    .FindAsIQueryable(p => p.id == id)
                    .FirstOrDefaultAsync();
                if (model == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent"));
                }

                model.delete_status = (int)SystemDataStatus.Invalid;
                model.modify_id = UserContext.Current.UserId;
                model.modify_name = UserContext.Current.UserName;
                model.modify_date = DateTime.Now;

                _repository.Update(model, x => new
                {
                    x.delete_status,
                    x.modify_id,
                    x.modify_name,
                    x.modify_date
                });

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
        /// 启用/禁用：更新 enable 字段
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="enable">0 禁用；1 启用</param>
        /// <returns>操作结果</returns>
        public async Task<WebResponseContent> EnableAsync(Guid id, byte enable)
        {
            try
            {
                var model = await _repository
                    .FindAsIQueryable(p => p.id == id)
                    .FirstOrDefaultAsync();
                if (model == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("non_existent"));
                }
                model.modify_id = UserContext.Current.UserId;
                model.modify_name = UserContext.Current.UserName;
                model.modify_date = DateTime.Now;
                model.enable = enable;

                _repository.Update(model, x => new
                {
                    x.enable,
                    x.modify_id,
                    x.modify_name,
                    x.modify_date
                });

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

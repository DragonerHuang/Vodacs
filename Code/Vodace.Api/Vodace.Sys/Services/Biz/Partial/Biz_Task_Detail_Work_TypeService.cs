
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
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;

namespace Vodace.Sys.Services
{
    public partial class Biz_Task_Detail_Work_TypeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Task_Detail_Work_TypeRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Task_Detail_Work_TypeService(
            IBiz_Task_Detail_Work_TypeRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILocalizationService localizationService
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _localizationService = localizationService;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        #region 新增、修改、删除、查询逻辑代码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="preQualInfo"></param>
        /// <returns></returns>
        public WebResponseContent AddTaskWorkType(TaskWorkTypeDto dtoTaskWorkType)
        {
            try
            {
                if (dtoTaskWorkType == null)
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                }

                bool isExist = _repository.Exists(d => d.task_detail_id == dtoTaskWorkType.task_detail_id && d.work_type_id == dtoTaskWorkType.work_type_id);
                if (!isExist)
                {
                    Biz_Task_Detail_Work_Type bizTaskWorkType = _mapper.Map<Biz_Task_Detail_Work_Type>(dtoTaskWorkType);
                    //Biz_Task_Detail_Work_Type bizTaskWorkType = new Biz_Task_Detail_Work_Type();
                    //MapValueToEntity(dtoTaskWorkType, bizTaskWorkType);
                    bizTaskWorkType.id = Guid.NewGuid();
                    bizTaskWorkType.create_id = UserContext.Current.UserId;
                    bizTaskWorkType.create_name = UserContext.Current.UserName;
                    bizTaskWorkType.delete_status = (int)SystemDataStatus.Valid;
                    bizTaskWorkType.create_date = DateTime.Now;
                    _repository.Add(bizTaskWorkType, true);
                    string _msg = $"{_localizationService.GetString("save")}{_localizationService.GetString("success")}";
                    // return WebResponseContent.Instance.OK("Ok");
                    return WebResponseContent.Instance.OK(_msg);
                }
                //提示已存在
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("M_TaskService.AddTask 新增内容保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        /// <summary>
        /// 修改 TaskWorkType
        /// </summary>
        /// <param name="mTaskDto"></param>
        /// <returns></returns>
        public WebResponseContent EditTaskWorkType(TaskWorkTypeDto dtoTaskWorkType)
        {
            try
            {
                if (dtoTaskWorkType == null || dtoTaskWorkType.id.IsNullOrEmpty())
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                }

                Biz_Task_Detail_Work_Type dbEntity = _repository.Find(d => d.id == dtoTaskWorkType.id).FirstOrDefault();
                if (dbEntity != null)
                {
                    bool isExist = _repository.Exists(d => d.task_detail_id == dtoTaskWorkType.task_detail_id && d.work_type_id == dtoTaskWorkType.work_type_id);
                    if (!isExist)
                    {
                        MapValueToEntity(dtoTaskWorkType, dbEntity);
                        dbEntity.create_id = UserContext.Current.UserId;
                        dbEntity.modify_name = UserContext.Current.UserName;
                        dbEntity.modify_date = DateTime.Now;
                        var effRow = _repository.Update(dbEntity, true);
                        return CheckOperationResult(effRow);
                    }
                    //提示已存在
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("existent")}");
                }
                //提示不存在
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("M_TaskService.AddTask 修改内容保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        /// 失效 TaskWorkType
        /// </summary>
        /// <returns></returns>
        public WebResponseContent DelTaskWorkType(Guid guid)
        {
            try
            {
                if (guid.IsNullOrEmpty())
                {
                    return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("content_connot_be_empty")}");
                }

                var dbEntity = _repository.Find(d => d.id == guid && d.delete_status == (int)SystemDataStatus.Valid).FirstOrDefault();
                if (dbEntity != null)
                {
                    dbEntity.delete_status = (int)SystemDataStatus.Invalid;
                    dbEntity.modify_id = UserContext.Current.UserId;
                    dbEntity.modify_name = UserContext.Current.UserName;
                    dbEntity.modify_date = DateTime.Now;
                    var effRow = _repository.Update(dbEntity, true);
                    return CheckOperationResult(effRow);
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("M_TaskService.DelTaskWorkType 失效操作保存失败", ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        /// <summary>
        ///  检查结果是否成功
        /// </summary>
        /// <param name="effRow"></param>
        /// <returns></returns>
        private WebResponseContent CheckOperationResult(int affectRow)
        {
            if (affectRow > 0)
            {
                return WebResponseContent.Instance.OK($"{_localizationService.GetString("save")}{_localizationService.GetString("success")}");
            }
            else
            {
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("save")}{_localizationService.GetString("operation_failed")}");
            }
        }

        /// <summary>
        /// 获取TaskWorkType列表
        /// </summary>
        /// <param name="pdo"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetTaskWorkTypeList(PageDataOptions pdo)
        {
            List<Biz_Task_Detail_Work_Type> lisTasks = new List<Biz_Task_Detail_Work_Type>();
            try
            {

                //后续需要增加解析过滤条件
                var query = await _repository.FindAsIQueryable(x => x.delete_status == (int)SystemDataStatus.Valid).ToListAsync();
                var total = query.Count();
                //获取项目列表
                lisTasks = await repository.FindAsIQueryable(x => x.delete_status == (int)SystemDataStatus.Valid)
                   .OrderByDescending(x => x.create_date)
                   .Skip((pdo.Page - 1) * pdo.Rows)
                   .Take(pdo.Rows)
                   .ToListAsync();

                return WebResponseContent.Instance.OK("Ok", lisTasks);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this.GetType().ToString(), ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }

        }
        /// <summary>
        ///  根据Id 获取TaskWorkType信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetTaskWorkTypeById(Guid guid)
        {
            try
            {
                var dbEntiry = await _repository.FindAsync(d => d.delete_status == (int)SystemDataStatus.Valid && d.id == guid);
                return WebResponseContent.Instance.OK("OK", dbEntiry);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this.GetType().ToString(), ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        #endregion
    }
}

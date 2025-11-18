
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
    public partial class Biz_Rolling_Program_TaskService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Rolling_Program_TaskRepository _repository;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Rolling_Program_TaskService(
            IBiz_Rolling_Program_TaskRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILocalizationService localizationService
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _localizationService = localizationService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public WebResponseContent Add(RollingProgramTaskDto dto)
        {
            try
            {
                Biz_Rolling_Program_Task model = new Biz_Rolling_Program_Task();
                model.id = Guid.NewGuid();
                model.delete_status = (int)SystemDataStatus.Valid;
                model.create_id = UserContext.Current.UserId;
                model.create_name = UserContext.Current.UserName;
                model.create_date = DateTime.Now;

                //读取当前合约是否已存在版本号
                var list = _repository.Find(a => a.delete_status == (int)SystemDataStatus.Valid && a.project_id == dto.project_id && a.contract_id == dto.contract_id).OrderByDescending(a => a.create_date).ToList();
                if(list.Count > 0)
                    model.version = (int.Parse(list[0].version) + 1) + ".0";    //每次数字追加
                else
                    model.version = "1.0";

                model.project_id = dto.project_id;
                model.contract_id = dto.contract_id;
                model.customer = dto.customer;
                model.category = dto.category;
                model.version = dto.version;
                model.task_name = dto.task_name;
                model.remark = dto.remark;

                _repository.Add(model, true);

                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), model);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_Program_TaskService.Add", e);
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
                Log4NetHelper.Error("Biz_Rolling_Program_TaskService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
        public WebResponseContent Edit(RollingProgramTaskDto dto)
        {
            try
            {
                var model = _repository.Find(a => a.id == dto.id).FirstOrDefault();
                if (model != null)
                {
                    model.modify_id = UserContext.Current.UserId;
                    model.modify_name = UserContext.Current.UserName;
                    model.modify_date = DateTime.Now;

                    model.project_id = dto.project_id;
                    model.contract_id = dto.contract_id;
                    model.customer = dto.customer;
                    model.category = dto.category;
                    model.version = dto.version;
                    model.task_name = dto.task_name;
                    model.remark = dto.remark;

                    var res = _repository.Update(model, true);
                    if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                    else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));

                } else return WebResponseContent.Instance.OK(_localizationService.GetString("non_existent"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Rolling_Program_TaskService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}

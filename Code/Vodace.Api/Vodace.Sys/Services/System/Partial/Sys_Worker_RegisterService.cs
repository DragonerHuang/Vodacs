
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
using System;
using Microsoft.AspNetCore.Identity;
using Vodace.Core.ManageUser;

namespace Vodace.Sys.Services
{
    public partial class Sys_Worker_RegisterService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Worker_RegisterRepository _repository;//访问数据库
        private readonly ISys_ContactRepository _ContactRepository;
        //private readonly ISys_UserRepository _userRepository;

        [ActivatorUtilitiesConstructor]
        public Sys_Worker_RegisterService(
            ISys_Worker_RegisterRepository dbRepository,
            ISys_ContactRepository contactRepository,
            //ISys_UserRepository userRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _ContactRepository = contactRepository;
            //_userRepository = userRepository;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public WebResponseContent Register(Sys_Worker_RegisterDto registerDto)
        {
            try
            {
                if (registerDto == null) return WebResponseContent.Instance.Error("注册内容不能为空！");
                Sys_Worker_Register worker_Register = new Sys_Worker_Register
                {
                    id = Guid.NewGuid(),
                    stc_no = registerDto.Stc_No,
                    wrc_issued_start_date = registerDto.Wrc_Issued_Start_Date,
                    stc_issued_end_date = registerDto.Stc_Issued_End_Date,
                    work_type = registerDto.Work_Type,
                    wrc_no = registerDto.Wrc_No,
                    stc_issued_start_date = registerDto.Stc_Issued_Start_Date,
                    wrc_issued_end_date = registerDto.Wrc_Issued_End_Date,
                    delete_status = 0,
                    create_date = DateTime.Now,
                    create_name = UserContext.Current.UserName,
                    create_id = UserContext.Current.UserId,
                };
                _repository.Add(worker_Register, true);
                return WebResponseContent.Instance.OK("注册成功", worker_Register.id);
            }
            catch (Exception ex)
            {
                return WebResponseContent.Instance.Error("注册失败");
            }
        }

    }
}




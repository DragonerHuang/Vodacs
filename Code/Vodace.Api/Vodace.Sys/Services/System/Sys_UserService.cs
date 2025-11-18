/*
 * 此代码由框架生成，请勿随意更改
 */
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_UserService : ServiceBase<Sys_User, ISys_UserRepository>, ISys_UserService, IDependency
    {
        public Sys_UserService(ISys_UserRepository repository)
             : base(repository)
        {
            Init(repository);
        }
        public static ISys_UserService Instance
        {
            get { return AutofacContainerModule.GetService<ISys_UserService>(); }
        }
    }
}


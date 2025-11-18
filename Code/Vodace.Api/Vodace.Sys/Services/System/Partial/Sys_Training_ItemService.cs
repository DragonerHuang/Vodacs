
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

namespace Vodace.Sys.Services
{
    public partial class Sys_Training_ItemService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_Training_ItemRepository _repository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public Sys_Training_ItemService(
            ISys_Training_ItemRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
  }
}

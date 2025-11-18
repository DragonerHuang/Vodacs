
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
    public partial class Biz_LineService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_LineRepository _repository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public Biz_LineService(
            IBiz_LineRepository dbRepository,
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

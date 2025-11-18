
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_DictionaryListService : ServiceBase<Sys_Dictionary_List, ISys_DictionaryListRepository>
    , ISys_DictionaryListService, IDependency
    {
        public Sys_DictionaryListService(ISys_DictionaryListRepository repository)
        : base(repository)
        {
            Init(repository);
        }
        public static ISys_DictionaryListService Instance
        {
            get { return AutofacContainerModule.GetService<ISys_DictionaryListService>(); }
        }
    }
}

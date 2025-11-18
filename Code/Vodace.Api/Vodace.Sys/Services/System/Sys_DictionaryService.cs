
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_DictionaryService : ServiceBase<Sys_Dictionary, ISys_DictionaryRepository>, ISys_DictionaryService, IDependency
    {
        public Sys_DictionaryService(ISys_DictionaryRepository repository)
             : base(repository)
        {
            Init(repository);
        }
        public static ISys_DictionaryService Instance
        {
            get { return AutofacContainerModule.GetService<ISys_DictionaryService>(); }
        }
    }
}

/*
 *Author：jxx
 *contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Sys_QuartzOptionsService与ISys_QuartzOptionsService中编写
 */
using Vodace.Sys.IRepositories;
using Vodace.Sys.IServices;
using Vodace.Core.BaseProvider;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.Services
{
    public partial class Sys_QuartzOptionsService : ServiceBase<Sys_QuartzOptions, ISys_QuartzOptionsRepository>
    , ISys_QuartzOptionsService, IDependency
    {
        public Sys_QuartzOptionsService(ISys_QuartzOptionsRepository repository, Core.Localization.ILocalizationService localizationService, AutoMapper.IMapper mapper, Core.Hubs.IHomePageMessageSender messageSender)
        : base(repository)
        {
            Init(repository);
            _localizationService = localizationService;
            _mapper = mapper;
            _messageSender = messageSender;
        }
        public static ISys_QuartzOptionsService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_QuartzOptionsService>(); } }
    }
 }

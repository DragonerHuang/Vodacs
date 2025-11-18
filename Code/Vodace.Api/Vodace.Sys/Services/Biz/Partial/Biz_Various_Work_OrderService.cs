
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Linq;
using System.Linq.Expressions;
using Vodace.Core.BaseProvider;
using Vodace.Core.DBManager;
using Vodace.Core.Extensions;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories.Biz;
using Vodace.Sys.IServices;

namespace Vodace.Sys.Services
{
    public partial class Biz_Various_Work_OrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiz_Various_Work_OrderRepository _repository;//访问数据库
        private readonly IBiz_ContractRepository _repositoryContract;//访问数据库
        private readonly IBiz_QuotationRepository _repositoryQuotation;//访问数据库
        private readonly ILocalizationService _localizationService;
        private readonly IBiz_QuotationService _quotationService;
        private DbContext dbContext = DBServerProvider.DbContext;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public Biz_Various_Work_OrderService(
            IBiz_Various_Work_OrderRepository dbRepository,
            IBiz_ContractRepository repositoryContract,
            IBiz_QuotationRepository repositoryQuotation,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILocalizationService localizationService,
            IBiz_QuotationService quotationService)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _repositoryContract = repositoryContract;
            _repositoryQuotation = repositoryQuotation;
            _mapper = mapper;
            _localizationService = localizationService;
            _quotationService = quotationService;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }


        /// <summary>
        /// 新增合约
        /// </summary>
        /// <param name="m_Contract"></param>
        /// <returns></returns>
        /// <remarks>
        /// 都是新增的需求，创建时都需要重新创建一个新的Contract(合约)，并且所有资料都需要重新填写一份，新合约需要与当前的VO/WO进行Contract.id关联，新的vo/wo对应的contract里面的master需要对应到原来的id，需要同时创建一个Biz_Confirmed_Order以及Biz_Quotation
        /// VO(Various Order)：不包含当前合约内容，需要另外付费的合约订单
        /// WO(Work Order)：在当前合约内，新增的需求，不需要另外付费的合约订单
        /// </remarks>
        public WebResponseContent AddDetail(Various_Work_OrderDto dtoVarious_Work_Order)
        {
            try
            {
                if(dtoVarious_Work_Order.dtoContract == null || dtoVarious_Work_Order.dtoQuotation == null || dtoVarious_Work_Order.dtoVarious_Work_Order == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("edit_vo_wo_data_incomplete"));
                }
                DateTime nowTime = DateTime.Now;

                Biz_Contract dtoContract = _mapper.Map<Biz_Contract>(dtoVarious_Work_Order.dtoContract);

                if(dtoContract == null)
                    return WebResponseContent.Instance.Error(_localizationService.GetString("qn_add_contract_null"));

                if (dtoContract.project_id == null)
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                }

                ////判断编码是否已存在
                //if(_quotationService.CheckRepeatContract(dtoContract.contract_no))
                //{
                //    return WebResponseContent.Instance.Error(_localizationService.GetString("contract_no_repeat"));
                //}

                dtoContract.id = Guid.NewGuid();
                dtoContract.create_date = nowTime;
                dtoContract.create_id = UserContext.Current.UserId;
                dtoContract.create_name = UserContext.Current.UserName;
                dtoContract.delete_status = 0;
                _repositoryContract.Add(dtoContract);


                Biz_Quotation dtoQuotation = _mapper.Map<Biz_Quotation>(dtoVarious_Work_Order.dtoQuotation);
                dtoQuotation.id = Guid.NewGuid();
                dtoQuotation.contract_id = dtoContract.id;
                dtoQuotation.create_date = nowTime;
                dtoQuotation.create_id = UserContext.Current.UserId;
                dtoQuotation.create_name = UserContext.Current.UserName;
                dtoQuotation.delete_status = 0;
                _repositoryQuotation.Add(dtoQuotation);


                Biz_Various_Work_Order dtoVariousWorkOrder = _mapper.Map<Biz_Various_Work_Order>(dtoVarious_Work_Order.dtoVarious_Work_Order);
                dtoVariousWorkOrder.contract_id = dtoContract.id;
                dtoVariousWorkOrder.qn_id = dtoQuotation.id;
                dtoVariousWorkOrder.id = Guid.NewGuid();
                dtoVariousWorkOrder.create_date = nowTime;
                dtoVariousWorkOrder.create_id = UserContext.Current.UserId;
                dtoVariousWorkOrder.create_name = UserContext.Current.UserName;
                dtoVariousWorkOrder.delete_status = 0;
                _repository.Add(dtoVariousWorkOrder);

                _repository.SaveChanges();

                return WebResponseContent.Instance.OK("Ok");
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_Various_Work_OrderService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }




        /// <summary>
        /// 新增合约
        /// </summary>
        /// <param name="m_Contract"></param>
        /// <returns></returns>
        /// <remarks>
        /// 都是新增的需求，创建时都需要重新创建一个新的Contract(合约)，并且所有资料都需要重新填写一份，新合约需要与当前的VO/WO进行Contract.id关联，新的vo/wo对应的contract里面的master需要对应到原来的id，需要同时创建一个Biz_Confirmed_Order以及Biz_Quotation
        /// VO(Various Order)：不包含当前合约内容，需要另外付费的合约订单
        /// WO(Work Order)：在当前合约内，新增的需求，不需要另外付费的合约订单
        /// </remarks>
        public WebResponseContent Add(Biz_Various_Work_Order biz_Various_Work_Order)
        {
            try
            {
                if (biz_Various_Work_Order == null) return WebResponseContent.Instance.Error($"{_localizationService.GetString("add")} {biz_Various_Work_Order.vo_wo_type} {_localizationService.GetString("connot_be_empty")}");

                biz_Various_Work_Order.id = Guid.NewGuid();
                biz_Various_Work_Order.create_id = UserContext.Current.UserId;
                biz_Various_Work_Order.create_name = UserContext.Current.UserName;
                biz_Various_Work_Order.create_date = DateTime.Now;
                _repository.Add(biz_Various_Work_Order, true);
                return WebResponseContent.Instance.OK("Ok", biz_Various_Work_Order);
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("M_Various_Work_OrderService.Add", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 删除合约
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Del(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                Biz_Various_Work_Order biz_Various_Work_Order = repository.Find(p => p.id == guid).FirstOrDefault();
                if (biz_Various_Work_Order != null)
                {
                    biz_Various_Work_Order.delete_status = 1;
                    biz_Various_Work_Order.modify_id = UserContext.Current.UserId;
                    biz_Various_Work_Order.modify_name = UserContext.Current.UserName;
                    biz_Various_Work_Order.modify_date = DateTime.Now;
                    var res = _repository.Update(biz_Various_Work_Order, true);

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
                Log4NetHelper.Error("Biz_Various_Work_OrderService.Del", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }

        /// <summary>
        /// 修改合约
        /// </summary>
        /// <param name="m_Project"></param>
        /// <returns></returns>
        public WebResponseContent Edit(Biz_Various_Work_Order biz_Various_Work_Order)
        {
            try
            {
                if (string.IsNullOrEmpty(biz_Various_Work_Order.id.ToString()) || biz_Various_Work_Order.id == Guid.Empty) return WebResponseContent.Instance.Error(_localizationService.GetString("id_null"));
                biz_Various_Work_Order.modify_id = UserContext.Current.UserId;
                biz_Various_Work_Order.modify_name = UserContext.Current.UserName;
                biz_Various_Work_Order.modify_date = DateTime.Now;
                var res = _repository.Update(biz_Various_Work_Order, true);

                if (res > 0) return WebResponseContent.Instance.OK(_localizationService.GetString("edit") + _localizationService.GetString("success"));
                else return WebResponseContent.Instance.Error(_localizationService.GetString("edit") + _localizationService.GetString("failes"));
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("Biz_Various_Work_OrderService.Edit", e);
                return WebResponseContent.Instance.Error(_localizationService.GetString("system_error"));
            }
        }
    }
}

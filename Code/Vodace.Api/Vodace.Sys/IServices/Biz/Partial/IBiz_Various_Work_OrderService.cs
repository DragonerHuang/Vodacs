
using System;
using System.Linq.Expressions;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Various_Work_OrderService
    {
        WebResponseContent Add(Biz_Various_Work_Order biz_Various_Work_Order);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(Biz_Various_Work_Order biz_Various_Work_Order);

        WebResponseContent AddDetail(Various_Work_OrderDto dtoVarious_Work_Order);
    }
 }

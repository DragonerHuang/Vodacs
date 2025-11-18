
using Vodace.Core.BaseProvider;
using Vodace.Entity.DomainModels;
using Vodace.Core.Utilities;
using System.Linq.Expressions;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Upcoming_EventsService
    {
        WebResponseContent GetEventList();
    }
 }

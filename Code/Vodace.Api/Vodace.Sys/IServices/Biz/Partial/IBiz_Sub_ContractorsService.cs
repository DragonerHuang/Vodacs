
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface IBiz_Sub_ContractorsService
    {
        /// <summary>
        /// 获取合约客户下拉列表
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> GetContractorsDownList();
    }
 }

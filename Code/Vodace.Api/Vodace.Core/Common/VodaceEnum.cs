using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Common
{
    public class VodaceEnum
    {
    }

    /// <summary>
    /// VODACS公共参数
    /// </summary>
    public class VodacsCommonParams
    {
        /// <summary>
        /// Quotation Number
        /// </summary>
        /// <remarks>
        /// 报价编码
        /// </remarks>
        public static string QN = "QN";

        /// <summary>
        /// Contract Number
        /// </summary>
        /// <remarks>
        /// 合约编码
        /// </remarks>
        public static string CO = "CO";

        /// <summary>
        /// 不包含在合约内 Various Order
        /// </summary>
        /// <remarks>
        /// 合约以外，新增的合约
        /// </remarks>
        public static string VO = "VO";

        /// <summary>
        /// 包含在合约内 Work Order
        /// </summary>
        /// <remarks>
        /// 合约以外，新增的合约
        /// </remarks>
        public static string WO = "WO";

        /// <summary>
        /// 项目编码
        /// </summary>
        public static string PR = "PR";
    }
}

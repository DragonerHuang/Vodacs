using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum SignTypeEnum
    {
        /// <summary>
        /// 工人签名
        /// </summary>
        [Description("工人签名")]
        worker = 0,

        /// <summary>
        /// scr的执行合资格人员签署
        /// </summary>
        [Description("执行合资格人员签署")]
        scr_spq = 1,

        /// <summary>
        /// scr的WPIC检查后签署
        /// </summary>
        [Description("WPIC检查后签署")]
        scr_wpic = 2,

        /// <summary>
        /// cpd的执行合资格人员签署
        /// </summary>
        [Description("执行合资格人员签署")]
        cpd_spq = 3,

        /// <summary>
        /// cpd的工务督察人员签署
        /// </summary>
        [Description("工务督察人员签署")]
        cpd_check = 4,

        /// <summary>
        /// qdc的WPIC检查后签署
        /// </summary>
        [Description("WPIC检查后签署")]
        qdc_wpic = 5,

        /// <summary>
        /// cp的SIC CP(T)签署
        /// </summary>
        [Description("SIC CP(T)签署")]
        cp_siccp = 6,

        /// <summary>
        /// sic的SIC签署
        /// </summary>
        [Description("sic的SIC签署")]
        sic_sic = 7,
    }

    public static class SignTypeHelper
    {
        /// <summary>
        /// 获取值更对应的值
        /// </summary>
        /// <param name="signType"></param>
        /// <returns></returns>
        public static string GetShiftTypeStr(int signType)
        {
            switch (signType)
            {
                case (int)SignTypeEnum.worker:
                    return "工人签名";
                case (int)SignTypeEnum.scr_spq:
                    return "执行合资格人员签署";
                case (int)SignTypeEnum.scr_wpic:
                    return "WPIC检查后签署";
                case (int)SignTypeEnum.cpd_spq:
                    return "执行合资格人员签署";
                case (int)SignTypeEnum.cpd_check:
                    return "工务督察人员签署";
                case (int)SignTypeEnum.qdc_wpic:
                    return "WPIC检查后签署";
                case (int)SignTypeEnum.cp_siccp:
                    return "SIC CP(T)签署";
                case (int)SignTypeEnum.sic_sic:
                    return "SIC签署";
                default:
                    return "";
            }
        }
    }
}

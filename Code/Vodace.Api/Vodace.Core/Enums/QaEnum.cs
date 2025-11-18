using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum QaEnum
    {
        /// <summary>
        /// 0：预审问答；
        /// </summary>
        Pre_QA = 0,
        /// <summary>
        /// 1：投标问答
        /// </summary>
        Ten_QA = 1
    }

    public static class QaEnumHelper
    {
        public static string GetQaStr(int val)
        {
            switch (val)
            {
                case (int)QaEnum.Pre_QA:
                    return "预审问答";
                case (int)QaEnum.Ten_QA:
                    return "投标问答";
                default:
                    return "";
            }
        }
    }
}

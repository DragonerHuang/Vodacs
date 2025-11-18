using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum ShiftTypeEnum
    {
        /// <summary>
        /// 早更
        /// </summary>
        [Description("早更")]
        Morning = 0,

        /// <summary>
        /// 中更
        /// </summary>
        [Description("中更")]
        Afternoon = 1,

        /// <summary>
        /// 晚更
        /// </summary>
        [Description("晚更")]
        Night = 2
    }

    public static class ShiftTypeHelper
    {
        /// <summary>
        /// 获取值更对应的值
        /// </summary>
        /// <param name="shiftType"></param>
        /// <returns></returns>
        public static string GetShiftTypeStr(int shiftType)
        {
            switch (shiftType)
            {
                case 0:
                    return "Morning Shift";
                case 1:
                    return "Afternoon Shift";
                case 2:
                    return "Night Shift";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取值更对应的索引
        /// </summary>
        /// <param name="shiftKey"></param>
        /// <returns></returns>
        public static int GetShiftTypeIndex(string shiftKey)
        {
            switch (shiftKey)
            {
                case "Morning Shift":
                    return (int)ShiftTypeEnum.Morning;
                case "Afternoon Shift":
                    return (int)ShiftTypeEnum.Afternoon;
                case "Night Shift":
                    return (int)ShiftTypeEnum.Night;
                default:
                    return -1;
            }
        }


        /// <summary>
        /// 获取值更对应的文本
        /// </summary>
        /// <param name="shiftKey"></param>
        /// <returns></returns>
        public static string GetShiftTypeValue(string shiftKey)
        {
            switch (shiftKey)
            {
                case "Morning Shift":
                    return "TH日更";
                case "Afternoon Shift":
                    return "中更";
                case "Night Shift":
                    return "NTN夜更";
                default:
                    return string.Empty;
            }
        }
    }
}

using System.ComponentModel;

namespace Vodace.Core.Enums
{
    public enum QnDeadlineCompleteEnum
    {
        /// <summary>
        /// 未完成
        /// </summary>
        [Description("未完成")]
        Unfinished = 0,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finish = 1,
    }


    public static class QnDeadlineHelper
    {
        public static string GetQnDeadlineCompleteStr(int val)
        {
            switch (val)
            {
                case (int)QnDeadlineCompleteEnum.Unfinished:
                    return "未完成";
                case (int)QnDeadlineCompleteEnum.Finish:
                    return "已完成";
                default:
                    return "";
            }
        }
    }
}

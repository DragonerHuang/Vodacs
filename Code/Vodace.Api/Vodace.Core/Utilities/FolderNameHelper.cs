using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Utilities
{
    public static class FolderNameHelper
    {
        // 定义Windows系统中不允许出现在文件夹名中的字符
        private static readonly char[] InvalidChars = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

        // 系统保留名称（不区分大小写）
        private static readonly HashSet<string> ReservedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "con", "prn", "aux", "nul",
            "com1", "com2", "com3", "com4", "com5", "com6", "com7", "com8", "com9",
            "lpt1", "lpt2", "lpt3", "lpt4", "lpt5", "lpt6", "lpt7", "lpt8", "lpt9"
        };

        /// <summary>
        /// 判断字符串是否可以作为有效的文件夹名
        /// </summary>
        /// <param name="name">要检查的字符串</param>
        /// <returns>如果可以作为文件夹名则返回true，否则返回false</returns>
        public static bool IsValidFolderName(string name)
        {
            // 检查是否为null或空字符串
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // 检查是否包含非法字符
            if (name.IndexOfAny(InvalidChars) != -1)
                return false;

            // 检查是否为系统保留名称
            string trimmedName = name.TrimEnd('.'); // 移除末尾的点
            if (ReservedNames.Contains(trimmedName))
                return false;

            // 检查长度是否超过255个字符
            if (name.Length > 255)
                return false;

            return true;
        }

        /// <summary>
        /// 将字符串转换为有效的文件夹名
        /// </summary>
        /// <param name="name">要转换的字符串</param>
        /// <param name="replacementChar">用于替换非法字符的字符</param>
        /// <returns>转换后的有效文件夹名</returns>
        public static string ToValidFolderName(this string name, char replacementChar = '_')
        {
            if (string.IsNullOrWhiteSpace(name))
                return "NewFolder";

            // 替换非法字符
            StringBuilder sb = new StringBuilder(name);
            foreach (char c in InvalidChars)
            {
                sb.Replace(c, replacementChar);
            }
            string result = sb.ToString();

            // 处理系统保留名称
            string trimmedName = result.TrimEnd('.');
            if (ReservedNames.Contains(trimmedName))
            {
                result = result + replacementChar;
            }

            // 处理过长的名称
            if (result.Length > 255)
            {
                result = result.Substring(0, 255);
            }

            // 确保不是空白字符串
            if (string.IsNullOrWhiteSpace(result))
            {
                result = "NewFolder";
            }

            return result;
        }
    }
}

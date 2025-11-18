using System;
using System.Collections.Generic;
using System.Text;

namespace Vodace.Core.Enums
{
    public enum UserAgent
    {
        IOS = 0,
        Android = 1,
        Windows = 2,
        Linux
    }

    public enum UserStatus 
    {
        /// <summary>
        /// 禁用
        /// </summary>
        disable = 0,
        /// <summary>
        /// 启用
        /// </summary>
        enable =1,
    }
}

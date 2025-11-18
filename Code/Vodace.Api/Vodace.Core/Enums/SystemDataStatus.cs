using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    /// <summary>
    /// 数据记录状态
    /// </summary>
    public enum SystemDataStatus
    {
       
        /// <summary>
        /// 有效
        /// </summary>
        Valid = 0, 
        /// <summary>
        /// 无效
        /// </summary>
        Invalid = 1,
        /// <summary>
        /// 从数据库手动删除
        /// </summary>
        ManualDelete = 2,
        /// <summary>
        /// 完成关闭
        /// </summary>
        CompleteClose = 3
    }

    public enum UserRegisterStatus 
    {
        /// <summary>
        /// 注册中(暂存)
        /// </summary>
        Registering=0,
        /// <summary>
        /// 注册完成
        /// </summary>
        Complete=1,
    }
}

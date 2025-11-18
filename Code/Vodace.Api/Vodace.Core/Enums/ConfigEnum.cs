using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum ConfigEnum
    {
        [Description("消息配置")]
        Message = 0,
        [Description("文件存放路径")]
        FilePath = 1,
        [Description("是否启用香港财政年")]
        IsHKFiscalYear = 2,
        [Description("使用的货币符号")]
        Currency = 3,
    }
}

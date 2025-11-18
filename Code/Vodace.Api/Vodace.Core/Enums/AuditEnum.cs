using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum AuditEnum
    {
        /// <summary>
        /// 待审核
        /// </summary>
        WaitAudit = 0,
        /// <summary>
        /// 审核中
        /// </summary>
        UnderReview = 1,
        /// <summary>
        /// 审核通过
        /// </summary>
        Passed =2,
        /// <summary>
        /// 驳回
        /// </summary>
        Reject=3,
        /// <summary>
        /// 停用
        /// </summary>
        Close =4
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum SubmissionFileEnum
    {
        /// <summary>
        /// 提交文件
        /// </summary>
        [Description("提交文件")]
        MasterFile = 0,

        /// <summary>
        /// 编辑文件
        /// </summary>
        [Description("编辑文件")]
        EditFile = 1,

        /// <summary>
        /// 客户评语
        /// </summary>
        [Description("客户评语")]
        CustomerReview = 2,

        /// <summary>
        /// 参考文件
        /// </summary>
        [Description("资料文件")]
        ReferenceFile = 3,

        /// <summary>
        /// 内部验收凭证
        /// </summary>
        [Description("内部验收凭证")]
        InternalAcceptance = 4,

        /// <summary>
        /// 客户验收凭证
        /// </summary>
        [Description("客户验收凭证")]
        CustomerAcceptance = 5,

        /// <summary>
        /// 开工前
        /// </summry>
        [Description("开工前")]
        BeforeWork = 6,

        /// <summary>
        /// 施工中
        /// </summry>
        [Description("施工中")]
        Working = 7,

        /// <summary>
        /// 完工后
        /// </summry>
        [Description("完工后")]
        AfterWork = 8,

        /// <summary>
        /// 要求文件
        /// </summary>
        [Description("要求文件")]
        DocFile = 9,

        /// <summary>
        /// 照片
        /// </summary>
        [Description("照片")]
        ImageFile = 10,

        /// <summary>
        /// 工人照片
        /// </summry>
        [Description("工人照片")]
        EmpPhoto,
    }

    public enum InnerStatusEnum
    {
        /// <summary>
        /// 编辑中
        /// </summary>
        [Description("编辑中")]
        Editing = 0,

        /// <summary>
        /// 审核中
        /// </summary>
        [Description("审批中")]
        UnderReview = 1,

        /// <summary>
        /// 已批核
        /// </summary>
        [Description("已批核")]
        Submitted = 2
    }

    public enum SubmissionFileStatusEnum
    {
        /// <summary>
        /// 待办
        /// </summary>
        [Description("待办")]
        ToDo = 0,

        /// <summary>
        /// 已提交
        /// </summary>
        [Description("已提交")]
        Submitted = 1,

        /// <summary>
        /// 已批准
        /// </summary>
        [Description("已批准")]
        Approved = 2,

        /// <summary>
        /// 条件批准
        /// </summary>
        [Description("条件批准")]
        ConditionalApproval = 3,

        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject = 4,

        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Cancel = 5,

        /// <summary>
        /// 不需要回应
        /// </summary>
        [Description("不需要回应")]
        NoResponse = 6
    }

    //public enum DeadlineManagementFileEnum
    //{
    //    /// <summary>
    //    /// 要求文件
    //    /// </summary>
    //    [Description("要求文件")]
    //    DocFile = 0,
    //    /// <summary>
    //    /// 照片
    //    /// </summary>
    //    [Description("照片")]
    //    ImageFile = 1,
    //}

    public static class SubmissionTypeEnumHelper
    {
        public static string GetSubmissionFileStr(int val)
        {
            switch (val)
            {
                case (int)SubmissionFileEnum.MasterFile:
                    return "主文件";

                case (int)SubmissionFileEnum.EditFile:
                    return "编辑文件";

                case (int)SubmissionFileEnum.CustomerReview:
                    return "客户评语";

                case (int)SubmissionFileEnum.ReferenceFile:
                    return "参考文件";

                case (int)SubmissionFileEnum.InternalAcceptance:
                    return "内部验收凭证";

                case (int)SubmissionFileEnum.CustomerAcceptance:
                    return "客户验收凭证";

                case (int)SubmissionFileEnum.BeforeWork:
                    return "开工前";

                case (int)SubmissionFileEnum.Working:
                    return "施工中";

                case (int)SubmissionFileEnum.AfterWork:
                    return "完工后";

                default:
                    return "";
            }
        }

        public static string GetSubmissionFileStrType(int val)
        {
            switch (val)
            {
                case (int)SubmissionFileEnum.BeforeWork:
                    return UploadFileCode.Site_Work_Photo_Before_Work;

                case (int)SubmissionFileEnum.Working:
                    return UploadFileCode.Site_Work_Photo_Working;

                case (int)SubmissionFileEnum.AfterWork:
                    return UploadFileCode.Site_Work_Photo_After_Work;

                default:
                    return "";
            }
        }
    }
}
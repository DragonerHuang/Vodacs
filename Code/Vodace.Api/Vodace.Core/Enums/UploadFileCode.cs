using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    /// <summary>
    /// 上传文件类别代码
    /// </summary>
    /// <remarks>
    /// 这里的编码Sys_File_Config适用于这里面的file_code，尽可能不修改，只做添加，如果非要修改，则需要同步修改Sys_File_Config表
    /// </remarks>
    public static class UploadFileCode
    {
        /// <summary>
        /// 建造行业安全训练证明书
        /// </summary>
        public static string STC = "wr_stc";

        /// <summary>
        /// 工人注册证
        /// </summary>
        public static string WRC = "wr_wrc";

        /// <summary>
        /// 报价新增时上传的合同资料
        /// </summary>
        public static string CTR = "ctr_file";

        /// <summary>
        /// 财务类文件
        /// </summary>
        public static string Financial_Documents = "Financial_Documents";

        /// <summary>
        /// 技术类文件
        /// </summary>
        public static string Technical_Documents = "Technical_Documents";

        /// <summary>
        /// 提交完成文件
        /// </summary>
        public static string Submit_Finish_Documents = "Submit_Finish_Documents";

        /// <summary>
        /// 报价单文件
        /// </summary>
        public static string Quotation_Record_Documents = "Quotation_Record_Documents";

        /// <summary>
        /// 投标问答文件
        /// </summary>
        public static string Tender_QA_Documents = "Tender_QA_Documents";

        /// <summary>
        /// 预审问答文件
        /// </summary>
        public static string Preliminary_Enquiry_QA_Documents = "Preliminary_Enquiry_QA_Documents";

        /// <summary>
        /// 标书补充文件
        /// </summary>
        public static string Tender_Add_Addendum = "Tender_Add_Addendum";

        /// <summary>
        /// 确认订单上传的文件
        /// </summary>
        public static string Qn_Co = "Qn_Co";

        /// <summary>
        /// 预审问答完成文件
        /// </summary>
        public static string Preliminary_Enquiry_QA_Finsih_Documents = "Preliminary_Enquiry_QA_Finsih_Documents";

        /// <summary>
        /// 投标问答完成文件
        /// </summary>
        public static string Tender_QA_Finish_Documents = "Tender_QA_Finish_Documents";

        /// <summary>
        /// 招标面试完成文件
        /// </summary>
        public static string Tender_Interview_Finish_Documents = "Tender_Interview_Finish_Documents";

        /// <summary>
        /// 现场考察完成文件
        /// </summary>
        public static string Site_Visit_Documents = "Site_Visit_Documents";

        /// <summary>
        /// 工程内容初始化文件
        /// </summary>
        public static string Construction_Content_Init = "Construction_Content_Init";

        /// <summary>
        /// 开工前
        /// </summary>
        public static string Site_Work_Photo_Before_Work = "Site_Work_Photo_Before_Work";

        /// <summary>
        /// 施工中
        /// </summary>
        public static string Site_Work_Photo_Working = "Site_Work_Photo_Working";

        /// <summary>
        /// 完工后
        /// </summary>
        public static string Site_Work_Photo_After_Work = "Site_Work_Photo_After_Work";

        /// <summary>
        /// 工地工作记录
        /// </summary>
        public static string Site_Work_Record = "Site_Work_Record";

        /// <summary>
        /// 提交文件-主目录
        /// </summary>
        public static string Submission_Files = "Submission";

        /// <summary>
        /// 提交文件-资料文件
        /// </summary>
        public static string Submission_Files_Information = "Submission_Information";
        /// <summary>
        /// 提交文件-修改文件
        /// </summary>
        public static string Submission_Files_Edit = "Submission_Edit";
        // <summary>
        /// 提交文件-提交文件
        /// </summary>
        public static string Submission_Files_Submit = "Submission_Submit";
        // <summary>
        /// 提交文件-客户评论
        /// </summary>
        public static string Submission_Files_Comments = "Submission_Comments";
        /// <summary>
        /// 竣工验收文件
        /// </summary>
        public static string Completion_Acceptance = "Completion_Acceptance";

        /// <summary>
        /// 期限管理文件-主目录
        /// </summary>
        public static string Deadline_Management = "Deadline_Management";
        /// <summary>
        /// 期限管理文件-图片
        /// </summary>
        public static string Deadline_Management_img = "Deadline_Management_Img";
        /// <summary>
        /// 期限管理文件-文档
        /// </summary>
        public static string Deadline_Management_doc = "Deadline_Management_Doc";

        /// <summary>
        /// 人事管理-员工管理
        /// </summary>
        public static string Employee_Management = "Employee_Management";
    }

    /// <summary>
    /// 上传状态，0：上传，1：完成（防止用户上传了，但是业务取消了）
    /// </summary>
    public enum UploadStatus
    {
        /// <summary>
        /// 上传
        /// </summary>
        Upload = 0,

        /// <summary>
        /// 完成
        /// </summary>
        Finish = 1
    }
}
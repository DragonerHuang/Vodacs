
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Entity.SystemModels;

namespace Vodace.Entity.DomainModels
{
    
    public partial class Biz_Submission_Files
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class SubmissionFilesQuery: SubmissionFilesByUserQuery
    {
        public Guid? contract_id { get; set; }
    }
    public class SubmissionFilesByUserQuery
    {
        public string file_no { get; set; }
        public string describe { get; set; }
    }

    public class SubmissionFilesEditDto : SubmissionFilesAddDto 
    {
        public Guid id { get; set; }
        public string describe { get; set; }
        public DateTime? approved_date { get; set; }
        public DateTime? actual_upload_date { get; set; }
        public int inner_status { get; set; }
        public string remark { get; set; }
        public string brand { get; set; }
        public int? to_email_user { get; set; }
        public string cc_email_users { get; set; }

    }
    public class SubmissionFilesAddDto
    {
        public Guid contract_id { get; set; }
        public string main_type { get; set; }
        public string child_type { get; set; }
        public string file_no { get; set;}
        public int? producer_id { get; set; }
        public int? approved_id { get; set; }
        public DateTime expected_upload_date { get; set; }
        public string describe { get; set; }
    }
    public class SubmissionFilesListExDto: SubmissionFilesListDto
    {
        public string contract_no { get; set; }
        public Guid? file_id { get; set; }
    }
    public class SubmissionFilesListDto
    {
        public Guid id { get; set; }

        /// <summary>
        ///提交编号
        /// </summary>
        public string file_no { get; set; }
        public string main_type { get; set; }

        /// <summary>
        ///版本
        /// </summary>
        public int version { get; set; }
        public string version_str { get; set; }

        /// <summary>
        ///内部状态（0：已提交；1：编辑中；2：审核种；3：代办；4：取消）
        /// </summary>
        public int inner_status { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        ///制作人
        /// </summary>
        public int? producer_id { get; set; }
        public string producer_name { get; set; }

        /// <summary>
        ///审核人
        /// </summary>
        public int? approved_id { get; set; }
        public string approved_name { get; set; }

        /// <summary>
        ///审核日期
        /// </summary>
        public DateTime? approved_date { get; set; }


        /// <summary>
        ///预计上传日期
        /// </summary>
        public DateTime? expected_upload_date { get; set; }

        /// <summary>
        ///实际上传日期
        /// </summary>
        public DateTime? actual_upload_date { get; set; }

        /// <summary>
        ///提交状态
        /// </summary>
        public int? submit_status { get; set; }

        /// <summary>
        ///合约Id
        /// </summary>
        public Guid? contract_id { get; set; }
        public DateTime? create_date { get; set; }
        public Guid? file_id { get; set; }
    }

    public class SubmissionAuditDto
    {
        public Guid[] id { get; set; }
        //public int status { get; set; }
    }

    public class SubmissionFilesRecordListDto
    {

        public Guid id { get; set; }

        /// <summary>
        ///提交文件id
        /// </summary>
        public Guid? submission_id { get; set; }

        /// <summary>
        ///版本
        /// </summary>
        public int version { get; set; }
        public string version_str { get; set; }

        /// <summary>
        ///内部状态（0：已提交；1：编辑中；2：审核种；3：代办；4：取消）
        /// </summary>
        public int inner_status { get; set; }

        /// <summary>
        ///上传人
        /// </summary>
        public int? upload_user_id { get; set; }
        public string upload_user_name { get; set; }

        /// <summary>
        ///提交时间
        /// </summary>
        public DateTime? submit_date { get; set; }

        /// <summary>
        ///提交状态
        /// </summary>
        public int? submit_status { get; set; }

        /// <summary>
        ///提交文件
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        ///文件路径
        /// </summary>
        //public string file_path { get; set; }

        /// <summary>
        ///文件类型（0：主文件；1：编辑文件；2：客户评语；3：参考文件）
        /// </summary>
        public int? file_type { get; set; }
        /// <summary>
        ///检查清单
        /// </summary>
        public string check_list { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? modify_date { get; set; }
        public int? file_version { get; set; }

    }

    public class UploadSubmissionFilesRecordDto
    {
        public Guid subId { get; set; }
        public int version { get; set; }
        public string check_list { get; set; }
        public int file_type { get; set; }
    }
    public class SubmissionRenameFileDto
    {
        public Guid subId { get; set; }
        public Guid file_Id { get; set; }
        public string file_name { get; set; }
    }
    public class UploadByApprovedDto
    {
        public Guid subId { get; set; }
        public int version { get; set; }
        public Guid file_Id { get; set; }
        public string check_list { get; set; }
    }
    public class UploadSubmissionFilesBaseDto 
    {
        public Guid subId { get; set; }
        public int version { get; set; }
        public int file_type { get; set; }
    }
    public class UploadSubmissionFilesRecordNewDto: UploadSubmissionFilesBaseDto
    {
        public string check_list { get; set; }
    }
    public class SubmissionFilesCopyDto 
    {
        public Guid subId { get; set; }
        public Guid file_Id { get; set; }
        /// <summary>
        /// 目标版本
        /// </summary>
        public int target_version { get; set; }
        /// <summary>
        /// 目标文件类型
        /// </summary>
        public int target_type { get; set; }
    }
    public class SubmissionFilesCheckDto
    {
        public Guid contract_id { get; set; }
        public string fileNo { get; set; }
        public string mainType { get; set; }
        public string childType { get; set; }
    }

    public class UploadSubmissionFilesCoverDto 
    {
        public Guid subId { get; set; }
        public Guid file_Id { get; set;}
        public int file_type { get; set; }
    }
}
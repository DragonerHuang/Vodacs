
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
    
    public partial class Biz_Deadline_Management
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class DeadlineManagementEditDto : DeadlineManagementAddDto
    {
        public Guid id { get; set; }
    }
    public class UFileInfoDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid? id { get; set; }
        /// <summary>
        /// 文件备注
        /// </summary>
        public string file_remark { get; set; }
    }
    public class DeadlineManagementAddDto
    {
        /// <summary>
        ///合约Id
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        ///期限类型（3：预审；4：现场考察；5：招标；6：公开招标；7：预审问答；8：邀请招标；9：招标问答；10：面试	）
        /// </summary>
        public int? deadline_type { get; set; }

        /// <summary>
        ///主题
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        ///预计日期
        /// </summary>
        public DateTime? estimated_date { get; set; }

        /// <summary>
        ///截至日期
        /// </summary>
        public DateTime? deadline_date { get; set; }

        /// <summary>
        ///实际日期
        /// </summary>
        public DateTime? actual_date { get; set; }

        /// <summary>
        ///客户联系人
        /// </summary>
        public string customer_contact { get; set; }

        /// <summary>
        ///负责人
        /// </summary>
        public int? director_user_id { get; set; }

        /// <summary>
        ///审批人
        /// </summary>
        public int? approved_id { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }
        public int? status { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        //public string remark { get; set; }
        //public List<FileInfoDto> docFile { get; set; }
        public List<UFileInfoDto> docFile { get; set; }
        public List<UFileInfoDto>  imgFile { get; set; }
    }

    public class DeadlineManagementModelDto: DeadlineManagementListDto
    {
        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
        public List<FileModel> doc_files { get; set; }
        public List<Guid> img_files { get; set; }
    }
    public class FileModel 
    {
        public Guid id { get; set; }
        public string file_name { get; set; }
        public string remark { get; set; }
        public int? file_size { get; set; }
    }
    public partial class DeadlineManagementListDto
    {
        public Guid id { get; set; }
        /// <summary>
        ///合约Id
        /// </summary>
        public Guid? contract_id { get; set; }

        /// <summary>
        ///期限类型（3：预审；4：现场考察；5：招标；6：公开招标；7：预审问答；8：邀请招标；9：招标问答；10：面试	）
        /// </summary>
        public int? deadline_type { get; set; }

        /// <summary>
        ///主题
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        ///预计日期
        /// </summary>
        public DateTime? estimated_date { get; set; }

        /// <summary>
        ///截至日期
        /// </summary>
        public DateTime? deadline_date { get; set; }

        /// <summary>
        ///实际日期
        /// </summary>
        public DateTime? actual_date { get; set; }

        /// <summary>
        ///客户联系人
        /// </summary>
        public string customer_contact { get; set; }

        /// <summary>
        ///负责人
        /// </summary>
        public int? director_user_id { get; set; }
        public string director_user_name { get; set; }

        /// <summary>
        ///审批人
        /// </summary>
        public int? approved_id { get; set; }
        public string approved_name { get; set; }

        /// <summary>
        ///审核日期
        /// </summary>
        //public DateTime? approved_date { get; set; }

        /// <summary>
        ///状态（0：待审核；1：已批准；2：驳回）
        /// </summary>
        //public int? status { get; set; }

        /// <summary>
        ///描述
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        //public string remark { get; set; }
        public int img_count { get; set; }
        public int doc_count { get; set; }
    }

    public class UploadDto
    {
        public Guid? deadline_id { get; set; }
        public int? file_type { get; set; }
    }


    public class DeadlineFileDto
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid? file_id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// 文件描述
        /// </summary>
        public string file_remark { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int file_size { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string file_path { get; set; }

    }
}
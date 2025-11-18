
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
    
    public partial class Biz_Completion_Acceptance_Record
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class CompletionAcceptanceRecordListDto
    {

        public Guid id { get; set; }

        /// <summary>
        ///提交文件id
        /// </summary>
        public Guid? acceptance_id { get; set; }

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

    }

    public class UploadAcceptanceRecordDto
    {
        public Guid acceptance_id { get; set; }
        public int version { get; set; }
        public string check_list { get; set; }
        public int file_type { get; set; }
    }

    public class UploadAcceptanceRecordExDto
    {
        public Guid acceptance_id { get; set; }
        public int file_type { get; set; }
    }
}

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
    
    public partial class Sys_File_Config
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }
    public class AddFileConfigDto
    {
        /// <summary>
        ///文件类型代码
        /// </summary>
        public string file_code { get; set; }

        /// <summary>
        ///存放的文件夹（相对位置）
        /// </summary>
        public string folder_path { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
    public class EditFileConfigDto : AddFileConfigDto
    {
        public Guid? id { get; set; }
    }
    public class FileConfigDto : EditFileConfigDto
    {
        public string remark { get; set; }
        public DateTime? create_date { get; set; }
        public string create_name { get; set; }
        public DateTime? modify_date { get; set; }
        public string modify_name { get; set; }

    }
}
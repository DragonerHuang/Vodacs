
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
    
    public partial class Sys_Dictionary
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class DictionaryEditDto:DictionaryDto
    { 
        public int dic_id { get; set; }
    }

    public class DictionaryDto
    {
        public int parent_id { get; set; }
        //public string config { get; set; }
        //public string db_server { get; set; }
        public string db_sql { get; set; }
        public string dic_name { get; set; }
        public string dic_no { get; set; }
        public byte enable { get; set; }
        public string remark { get; set; }
        public int? order_no { get; set; }
        public List<DictionaryListDto> dictionaryList { get; set; }
    }

    public class DictionaryListDto 
    {
        public int dic_list_id { get; set; }
        public string dic_name { get; set; }
        public string dic_name_eng { get; set; }
        public string dic_value { get; set; }
        public byte? enable { get; set; }
        public int? order_no { get; set; }
        public string remark { get; set; }
    }

    public class DictionarySearchDto
    {
        /// <summary>
        /// 字典编号
        /// </summary>
        public string dic_name { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string dic_no { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int? parent_id { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public byte? enable { get; set; }

        /// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime? create_date { get; set; }

        ///// <summary>
        ///// 修改时间
        ///// </summary>
        //public DateTime? modify_date { get; set; }
    }


    //public partial class Sys_Dictionary : BaseEntity
    //{
    //    public int dic_id { get; set; }

    //    public int parent_id { get; set; }

    //    public string dic_name { get; set; }

    //    public string dic_no { get; set; }


    //    public byte enable { get; set; }


    //    public string remark { get; set; }


    //    public int? delete_status { get; set; }


    //    public int? create_id { get; set; }


    //    public string create_name { get; set; }


    //    public DateTime? create_date { get; set; }

    //    public int? modify_id { get; set; }

    //    public string modify_name { get; set; }

    //    public DateTime? modify_date { get; set; }


    //    public List<Sys_Dictionary_List> Sys_DictionaryList { get; set; }

    //}
}
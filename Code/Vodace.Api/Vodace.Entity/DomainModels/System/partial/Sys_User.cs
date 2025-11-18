/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
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
    
    public partial class Sys_User
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class UserNewDto 
    {
        public Guid id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int user_id { get; set; }


        /// <summary>
        ///
        /// </summary>
        public int role_id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string phone_no { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string user_true_name { get; set; }


        /// <summary>
        ///
        /// </summary>
        public string user_name_eng { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? lang { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string email { get; set; }

        /// <summary>
        ///
        /// </summary>
        public byte enable { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int? gender { get; set; }
    }
}
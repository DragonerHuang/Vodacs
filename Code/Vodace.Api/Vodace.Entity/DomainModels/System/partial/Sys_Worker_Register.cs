
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
    
    public partial class Sys_Worker_Register
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class Sys_Worker_RegisterDto 
    {
        public string Stc_No { get; set; }

        /// <summary>
        ///签发日期
        /// </summary>
        public DateTime Stc_Issued_Start_Date { get; set; }

        /// <summary>
        ///有效日期
        /// </summary>
        public DateTime Stc_Issued_End_Date { get; set; }

        /// <summary>
        ///工人注册证
        /// </summary>
        public string Wrc_No { get; set; }

        /// <summary>
        ///签发日期
        /// </summary>
        public DateTime Wrc_Issued_Start_Date { get; set; }

        /// <summary>
        ///有效日期
        /// </summary>
        public DateTime Wrc_Issued_End_Date { get; set; }

        /// <summary>
        ///专业工种
        /// </summary>
        public Guid Work_Type { get; set; }
        public Guid[] FileIds { get; set; }
    }
}
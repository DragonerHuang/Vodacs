
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
    [Entity(TableCnName = "Biz_Contract_Details ",TableName = "Biz_Contract_Details")]
    public partial class Biz_Contract_Details:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid id { get; set; }

       /// <summary>
       ///是否删除（0：正常；1：删除；2：数据库手删除）
       /// </summary>
       [Display(Name ="是否删除（0：正常；1：删除；2：数据库手删除）")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? delete_status { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? create_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string create_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="create_date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? create_date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_id")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? modify_id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_name")]
       [MaxLength(30)]
       [Column(TypeName="nvarchar(30)")]
       [Editable(true)]
       public string modify_name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="modify_date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? modify_date { get; set; }

       /// <summary>
       ///合约id
       /// </summary>
       [Display(Name ="合约id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public Guid contract_id { get; set; }

       /// <summary>
       ///（公开招标、邀请招标）招标发布日期（合同资料）
       /// </summary>
       [Display(Name ="（公开招标、邀请招标）招标发布日期（合同资料）")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? issue_date { get; set; }

       /// <summary>
       ///（公开招标、邀请招标）招标截止日期（合同资料）
       /// </summary>
       [Display(Name ="（公开招标、邀请招标）招标截止日期（合同资料）")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? end_date { get; set; }

       /// <summary>
       ///（公开招标）招标标题（合同资料、标书资料）
       /// </summary>
       [Display(Name ="（公开招标）招标标题（合同资料、标书资料）")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string title { get; set; }

       /// <summary>
       ///（邀请招标）参考编号
       /// </summary>
       [Display(Name ="（邀请招标）参考编号")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string pei_tender_ref { get; set; }

       /// <summary>
       ///（邀请招标）主题
       /// </summary>
       [Display(Name ="（邀请招标）主题")]
       [MaxLength(225)]
       [Column(TypeName="nvarchar(225)")]
       [Editable(true)]
       public string pei_subject { get; set; }

       /// <summary>
       ///（邀请招标）信息
       /// </summary>
       [Display(Name ="（邀请招标）信息")]
       [MaxLength(300)]
       [Column(TypeName="nvarchar(300)")]
       [Editable(true)]
       public string pei_info { get; set; }

       /// <summary>
       ///（公开招标）往来文件（合同资料、标书资料）
       /// </summary>
       [Display(Name ="（公开招标）往来文件（合同资料、标书资料）")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string trade { get; set; }

       /// <summary>
       ///（公开招标）预审资格截止日期（合同资料）
       /// </summary>
       [Display(Name ="（公开招标）预审资格截止日期（合同资料）")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? antic_pql_sub_close_date { get; set; }

       /// <summary>
       ///（公开招标）预计预审资格提交日期合同资料）
       /// </summary>
       [Display(Name ="（公开招标）预计预审资格提交日期合同资料）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string antic_pql_sub_date { get; set; }

       /// <summary>
       ///（公开招标）预计招标标邀请日期（合同资料）
       /// </summary>
       [Display(Name ="（公开招标）预计招标标邀请日期（合同资料）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string antic_inv_tndr_date { get; set; }

       /// <summary>
       ///（公开招标）招标公布日期（合同资料）
       /// </summary>
       [Display(Name ="（公开招标）招标公布日期（合同资料）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string antic_cntr_awd_date { get; set; }

       /// <summary>
       ///（公开招标）成本范围（合同资料）
       /// </summary>
       [Display(Name ="（公开招标）成本范围（合同资料）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string range_cost { get; set; }

       /// <summary>
       ///姓名(合同的联络人)（合同资料）
       /// </summary>
       [Display(Name ="姓名(合同的联络人)（合同资料）")]
       [MaxLength(250)]
       [Column(TypeName="nvarchar(250)")]
       [Editable(true)]
       public string contact_name { get; set; }

       /// <summary>
       ///头衔、职称(合同的联络人)（合同资料）
       /// </summary>
       [Display(Name ="头衔、职称(合同的联络人)（合同资料）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string contact_title { get; set; }

       /// <summary>
       ///邮箱(合同的联络人)（合同资料）
       /// </summary>
       [Display(Name ="邮箱(合同的联络人)（合同资料）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string contact_email { get; set; }

       /// <summary>
       ///电话(合同的联络人)（合同资料）
       /// </summary>
       [Display(Name ="电话(合同的联络人)（合同资料）")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(40)")]
       [Editable(true)]
       public string contact_tel { get; set; }

       /// <summary>
       ///传真(合同的联络人)（合同资料）
       /// </summary>
       [Display(Name ="传真(合同的联络人)（合同资料）")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       public string contact_fax { get; set; }

       /// <summary>
       ///标书发布日期（标书资料）
       /// </summary>
       [Display(Name ="标书发布日期（标书资料）")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? tender_start_date { get; set; }

       /// <summary>
       ///招标截止时间（标书资料）
       /// </summary>
       [Display(Name ="招标截止时间（标书资料）")]
       [Column(TypeName="date")]
       [Editable(true)]
       public DateTime? tender_end_date { get; set; }

       /// <summary>
       ///备注（标书资料）
       /// </summary>
       [Display(Name ="备注（标书资料）")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string tender_remark { get; set; }

       /// <summary>
       ///客户类型（新增时选择的）
       /// </summary>
       [Display(Name ="客户类型（新增时选择的）")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       public string customer_type { get; set; }

       
    }
}
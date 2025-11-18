
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
    
    public partial class Biz_Project
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class SearchProjectDto
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        public string strProjectNo { get; set; }
        /// <summary>
        /// 合约名称
        /// </summary>
        public string strContractName { get; set; }
        /// <summary>
        /// 合约编码
        /// </summary>
        public string strContractNo { get; set; }
        /// <summary>
        /// 站点位置
        /// </summary>
        public string strSiteName { get; set; }
    }

    public class ProjectDto
    {
        public Guid? id { get; set; }
        /// <summary>
        ///所属公司id
        /// </summary>
        public Guid? company_id { get; set; }

        /// <summary>
        ///所属上级项目ID
        /// </summary>
        public Guid? master_id { get; set; }

        /// <summary>
        ///上级新增项目ID（vo/wo）
        /// </summary>
        public Guid? original_id { get; set; }

        /// <summary>
        ///项目状态代码
        /// </summary>
        public int pro_type_id { get; set; }

        /// <summary>
        ///客户id
        /// </summary>
        public Guid? customer_id { get; set; }

        /// <summary>
        ///项目编号
        /// </summary>
        public string project_no { get; set; }

        /// <summary>
        ///简称
        /// </summary>
        public string name_sho { get; set; }

        /// <summary>
        ///英文名
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        ///中文名
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        ///别名
        /// </summary>
        public string name_ali { get; set; }

        /// <summary>
        ///期望开始时间
        /// </summary>
        public DateTime? exp_start_date { get; set; }

        /// <summary>
        ///实际开始时间
        /// </summary>
        public DateTime? act_start_date { get; set; }

        /// <summary>
        ///期望结束时间
        /// </summary>
        public DateTime? exp_end_date { get; set; }

        /// <summary>
        ///实际结束时间
        /// </summary>
        public DateTime? act_end_date { get; set; }
        /// <summary>
        /// 是否删除（0：正常；1：删除；2：数据库手删除）
        /// </summary>
        public int delete_status { get; set; } = 1;
        public DateTime? create_date { get; set; } = DateTime.Now;

        /// <summary>
        ///备注
        /// </summary>
        public string remark { get; set; }
    }

    /// <summary>
    /// 项目详情
    /// </summary>
    public class ProjectDetailDto : ProjectDto
    {
        /// <summary>
        /// 所属公司名称
        /// </summary>
        public string strCompanyName { get; set; }

        /// <summary>
        /// 上级新增项目名称
        /// </summary>
        public string strMasterProjectName { get; set; }

        /// <summary>
        /// 项目所属客户
        /// </summary>
        public string strCustomerName { get; set; }

        public int create_id { get; set; }
        public string create_name { get; set; }
        public int modify_id { get; set; }
        public string modify_name { get; set; }
        public DateTime? modify_date { get; set; }
    }

    public class ProjectListDto
    {
        public Guid id { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string project_no { get; set; }
        /// <summary>
        /// 项目英文名
        /// </summary>
        public string name_eng { get; set; }
        /// <summary>
        /// 项目中文名
        /// </summary>
        public string name_cht { get; set; }
        /// <summary>
        /// 项目期望开始时间
        /// </summary>
        public DateTime? exp_start_date { get; set; }
        /// <summary>
        /// 项目期望结束时间
        /// </summary>
        public DateTime? exp_end_date { get; set; }
        /// <summary>
        /// 统合施工地点
        /// </summary>
        public string strSiteName { get; set; }
        /// <summary>
        /// 合约名集
        /// </summary>
        public string strContractName { get; set; }
        /// <summary>
        /// 合约进度
        /// </summary>
        public string intContractProgress { get; set; }
        /// <summary>
        /// 项目关联的合约
        /// </summary>
        public List<ProjectContractDto> children { get; set; }
    }

    /// <summary>
    /// 项目关联的合约
    /// </summary>
    public class ProjectContractDto
    {
        /// <summary>
        /// 合约ID
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// 合约编码
        /// </summary>
        public string contract_no { get; set; }
        /// <summary>
        /// 合约英文名
        /// </summary>
        public string name_eng { get; set; }
        /// <summary>
        /// 合约中文名
        /// </summary>
        public string name_cht { get; set; }
        ///// <summary>
        ///// 合约发行日期
        ///// </summary>
        //public DateTime? issue_date { get; set; }
        ///// <summary>
        ///// 合约截止日期
        ///// </summary>
        //public DateTime? end_date { get; set; }
        /// <summary>
        /// 项目合约施工地点
        /// </summary>
        public List<ProjectContractSiteDto> sites { get; set; }
    }

    /// <summary>
    /// 项目合约施工地点
    /// </summary>
    public class ProjectContractSiteDto
    {
        /// <summary>
        /// 合约施工地点ID
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// 合约施工地点英文名
        /// </summary>
        public string name_eng { get; set; }
        /// <summary>
        /// 合约施工地点中文名
        /// </summary>
        public string name_cht { get; set; }
    }

    /// <summary>
    /// 项目下拉列表
    /// </summary>
    public class ProjectDownDto
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public Guid project_id { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string project_no { get; set; }

        /// <summary>
        /// 项目名（英语）
        /// </summary>
        public string name_eng { get; set; }

        /// <summary>
        /// 项目名（中文名）
        /// </summary>
        public string name_cht { get; set; }

        /// <summary>
        /// 项目名（别名）
        /// </summary>
        public string name_ali { get; set; }
    }

}
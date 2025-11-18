
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
    
    public partial class Sys_Menu
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    public class MenuTree 
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string component { get; set; }
        public string menuNameTW { get; set; }
        public string menuNameUS { get; set; }
        public string icon { get; set; }
        public string[] permission { get; set; }
        public int? orderNo { get; set; }
        public bool hidden { get; set; } = false;
        public string description { get; set; }
        public string parent_title { get; set; }
        public List<MenuTree> children { get; set; } = new List<MenuTree>();
    }


    public class MenuAddDto
    {
        public string icon { get; set; }
        public string menu_name { get; set; }
        public string menu_name_us { get; set; }
        public string menu_name_tw { get; set; }
        public string auth { get; set; }
        public string component { get; set; }
        public string description { get; set; }
        public byte? enable { get; set; } = 1;
        public int? parent_id { get; set; }
        public string url { get; set; }
        public int orderNo { get; set; }
        public bool hidden { get; set; } = false;
        public string parent_title { get; set; }
        //public int? menu_type { get; set; } = 3;
    }
    public class MenuDto 
    {
        public int? menu_id { get; set; }
        public string icon { get; set; }
        public string menu_name { get; set; }
        public string menu_name_us { get; set; }
        public string menu_name_tw { get; set; }
        public string auth { get; set; }
        public string component { get; set; }
        public string description { get; set; }
        public byte? enable { get; set; } = 1;
        public int? parent_id { get; set; }
        public string url { get; set; }
        public int orderNo { get; set; }
        public bool hidden { get; set; } = false;
        public string parent_title { get; set; }
        //public int? menu_type { get; set; } = 3;
    }
    public class MenuQuery
    {
        public string menu_name { get; set; }
        public int? enable { get; set; }
        public int parent_id { get; set; }
    }
    public class MenuListDto 
    {
        public int menu_id { get; set; }
        public string icon { get; set; }
        public string menu_name { get; set; }
        public string menu_name_us { get; set; }
        public string menu_name_tw { get; set; }
        public string auth { get; set; }
        public string component { get; set; }
        public string description { get; set; }
        public byte? enable { get; set; } = 1;
        public int parent_id { get; set; }
        public string url { get; set; }
        public bool hidden { get; set; } = false;
        public string parent_title { get; set; }
        public string create_name { get; set; }
        public DateTime? create_date { get; set; }
        public int? orderNo { get; set; }
        //public List<Sys_Actions> Actions { get; set; }
    }

    public class MenuHiddenDto
    {
        public int menu_id { get; set; }
        public bool hidden { get; set; } = false;
    }
}
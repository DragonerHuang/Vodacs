using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Entity.DomainModels.Common
{
    internal class ComModel
    {
    }
    public class ApiVersionConfig
    {
        public string CurrentVersion { get; set; } = "1.0";
        public bool EnableVersioning { get; set; } = true;
    }
    public class TreeNode
    {
        public string value { get; set; }
        public string lable { get; set; }
        public List<Children> children { get; set; } =new List<Children>();
    }

    public class Children
    {
        public string value { get; set; }
        public string lable { get; set; }
    }


    public class MenuTreeNode
    {
        public int value { get; set; }
        public string label { get; set; }
        public List<MenuChildren> children { get; set; } = new List<MenuChildren>();
    }

    public class MenuChildren
    {
        public int value { get; set; }
        public string label { get; set; }
    }
}

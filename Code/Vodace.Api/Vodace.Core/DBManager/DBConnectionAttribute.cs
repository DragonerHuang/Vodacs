using System;
using System.Collections.Generic;
using System.Text;

namespace Vodace.Core.DBManager
{
    public class DBConnectionAttribute : Attribute
    {
        public string DBName { get; set; }
    }
}

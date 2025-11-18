using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vodace.Entity
{
   public class Sys_Actions
    {
        [Key]
        //public int Action_Id { get; set; }
        //public int Menu_Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string text_eng { get; set; }
        public string text_cht { get; set; }
    }
}

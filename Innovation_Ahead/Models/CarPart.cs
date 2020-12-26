using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Innovation_Ahead.Models
{
    public class CarPart
    {
        public string carName { get; set; }
        public string makeyear { get; set; }
        [Required]
        public string sparePart { get; set; }
        public string link1 { get; set; }
        public string link2 { get; set; }

        public virtual UserRegister UserRegister { get; set; }
    }
}
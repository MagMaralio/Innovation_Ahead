using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Innovation_Ahead.Models
{
    public class car
    {
        [Required]
        public string clientName { get; set; }
        [Required]
        public string mobileNo { get; set; }
        public string carName { get; set; }
        public string makeyear { get; set; }
        [Required]
        public string sparePart { get; set; }
    }
}
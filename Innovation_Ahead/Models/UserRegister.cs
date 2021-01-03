using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Innovation_Ahead.Models
{
    public class UserRegister
    {
        public string email { get; set; }
        public string password { get; set; }
        public string mobileNo { get; set; }
        public string firm { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Innovation_Ahead.Models
{
    public class UserRegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string mobileNo { get; set; }
    }
}
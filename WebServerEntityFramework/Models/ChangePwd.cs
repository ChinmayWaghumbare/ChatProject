using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServerEntityFramework.Models
{
    public class ChangePwd
    {
        public int ID { get; set; }
        public string UID { get; set; }
        public string UPWD { get; set; }

        public string newUPWD { get; set; }
    }
}
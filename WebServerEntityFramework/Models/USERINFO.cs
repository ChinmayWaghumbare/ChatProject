//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebServerEntityFramework.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class USERINFO
    {
        public USERINFO()
        {
            this.MESSAGEMASTs = new HashSet<MESSAGEMAST>();
            this.MESSAGEMASTs1 = new HashSet<MESSAGEMAST>();
            this.ONLINEUSERS = new HashSet<ONLINEUSER>();
        }
    
        public int ID { get; set; }
        public string USER_NAME { get; set; }
        public int LOGIN_ID { get; set; }
    
        public virtual LOGIN LOGIN { get; set; }
        public virtual ICollection<MESSAGEMAST> MESSAGEMASTs { get; set; }
        public virtual ICollection<MESSAGEMAST> MESSAGEMASTs1 { get; set; }
        public virtual ICollection<ONLINEUSER> ONLINEUSERS { get; set; }
    }
}

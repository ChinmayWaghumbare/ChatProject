﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebServerEntityFramework.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ChatMasterEntities : DbContext
    {
        public ChatMasterEntities()
            : base("name=ChatMasterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<LOGIN> LOGINs { get; set; }
        public virtual DbSet<MESSAGEMAST> MESSAGEMASTs { get; set; }
        public virtual DbSet<ONLINEUSER> ONLINEUSERS { get; set; }
        public virtual DbSet<USERINFO> USERINFOes { get; set; }
        public virtual DbSet<CONNECTION> CONNECTIONS { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class ReMinderContext : DbContext
    {
        public ReMinderContext()
            : base("ReMinderContext")//ConnectionString Name
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        //public DbSet<QuestionArea> QuestionAreas { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
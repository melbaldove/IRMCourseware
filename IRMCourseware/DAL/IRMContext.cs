using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using IRMCourseware.Models;

namespace IRMCourseware.DAL
{
    public class IRMContext : DbContext
    {
        public IRMContext() : base("IRM")
        {

        }

        public DbSet<Document> Documents { get; set; }
        public DbSet <Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
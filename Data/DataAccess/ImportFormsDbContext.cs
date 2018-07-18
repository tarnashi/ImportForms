using Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;

namespace Data.DataAccess
{
    public class ImportFormsDbContext : DbContext
    {
        public ImportFormsDbContext() : base("ImportFormsDbConnectionString")
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}

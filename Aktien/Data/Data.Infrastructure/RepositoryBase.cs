using Data.Entity.AktieEntitys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infrastructure
{
    public class RepositoryBase : DbContext
    {

        public DbSet<Aktie> Aktien { get; set; }

        public DbSet<Dividende> Dividenden { get; set; }

        public RepositoryBase() : base() { this.Database.Migrate(); }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(VersionContextConnection.GetDatabaseConnectionstring());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

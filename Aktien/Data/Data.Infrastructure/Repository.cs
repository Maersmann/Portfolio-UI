using Aktien.Data.Model.AktienModels;
using Aktien.Data.Model.DepotModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aktien.Data.Infrastructure
{
    public class Repository : DbContext
    {

        public DbSet<Aktie> Aktien { get; set; }
        public DbSet<Dividende> Dividenden { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<Depot> Depots { get; set; }
        public DbSet<DepotAktie> AktienInDepots { get; set; }
        public DbSet<DividendeErhalten> ErhaltendeDividenden { get; set; }

        public Repository() : base() { this.Database.Migrate(); }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(VersionContextConnection.GetDatabaseConnectionstring());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}

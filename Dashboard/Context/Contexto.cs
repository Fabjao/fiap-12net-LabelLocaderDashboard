using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Context;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Context
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions options) : base(options) { }

        public Contexto() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryProvider");
        }


        public DbSet<OrderChanged> OrderChanged { get; set; }
        public DbSet<UserWithLessOffer> UserWithLessOffer { get; set; }

    }
}

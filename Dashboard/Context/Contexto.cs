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


        public DbSet<OrderChanged> OrderChanged { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("Dashbord");
        }
    }
}

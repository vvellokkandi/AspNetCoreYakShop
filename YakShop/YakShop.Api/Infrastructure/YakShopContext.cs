using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YakShop.Common.Models;

namespace YakShop.Api.Infrastructure
{
    public class YakShopContext : DbContext
    {
        public YakShopContext(DbContextOptions<YakShopContext> context) : base(context)
        {
        }

        public DbSet<LabYak> Herds { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Settings> Settings { get; set; }
       
    }  
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SupremeCourtDocketApp.Models
{
    public class SupremeCourtDocketSimpleAppContext : DbContext
    {
        public SupremeCourtDocketSimpleAppContext (DbContextOptions<SupremeCourtDocketSimpleAppContext> options)
            : base(options)
        {
        }

        public DbSet<SupremeCourtDocketApp.Models.SupremeCourtDocketSimple> SupremeCourtDocketSimple { get; set; }
    }
}

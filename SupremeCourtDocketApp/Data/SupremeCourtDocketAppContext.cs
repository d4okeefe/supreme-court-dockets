using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SupremeCourtDocketApp.Models
{
    public class SupremeCourtDocketAppContext : DbContext
    {
        public SupremeCourtDocketAppContext (DbContextOptions<SupremeCourtDocketAppContext> options)
            : base(options)
        {
        }

        public DbSet<SupremeCourtDocketApp.Models.SupremeCourtDocket> SupremeCourtDocket { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SupremeCourtDocketApp.Models;

namespace SupremeCourtDocketApp.Pages
{
    public class DocketsSimpleModel : PageModel
    {
        private readonly SupremeCourtDocketApp.Models.SupremeCourtDocketSimpleAppContext _context;

        public DocketsSimpleModel(SupremeCourtDocketApp.Models.SupremeCourtDocketSimpleAppContext context)
        {
            _context = context;
        }

        public IList<SupremeCourtDocketSimple> SupremeCourtDocketSimple { get;set; }

        public async Task OnGetAsync()
        {
            SupremeCourtDocketSimple = await _context.SupremeCourtDocketSimple.ToListAsync();
        }
    }
}

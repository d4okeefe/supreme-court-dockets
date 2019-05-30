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
    public class DocketsModel : PageModel
    {
        private readonly SupremeCourtDocketApp.Models.SupremeCourtDocketAppContext _context;

        public DocketsModel(SupremeCourtDocketApp.Models.SupremeCourtDocketAppContext context)
        {
            _context = context;
        }

        public IList<SupremeCourtDocket> SupremeCourtDocket { get;set; }
        public int TotalCount{ get; set; }

        public async Task OnGetAsync()
        {
            // _context.SupremeCourtDocket.Include(x=>x.Proceedings);
            // _context.SupremeCourtDocket.Include(x=>x.Contacts);
            SupremeCourtDocket = await _context
                .SupremeCourtDocket
                .Include(x=>x.Proceedings)
                .Include(x=>x.Contacts)
                .OrderBy(x=>x.DocketYear)
                .ThenBy(x=>x.DocketNoYear)
                .ToListAsync();
            
            

            TotalCount = SupremeCourtDocket.Count();
            
        }
    }
}

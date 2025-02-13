﻿using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using Microsoft.EntityFrameworkCore;
using METU.Admin.Model;
namespace METU.Admin.Pages.Manage.syspagedic
{
    public class DetailsModel : MetuPageBase
    {
        

        public DetailsModel(CoreBLL context)
        {
            _context = context;
        }

        public SyspageDic SyspageDic { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SyspageDic = await _context.dbh.Set<SyspageDic>().FirstOrDefaultAsync(m => m.id == id);

            if (SyspageDic == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

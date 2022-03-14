using Fenco.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fenco.ViewModels;

namespace Fenco.ViewComponents
{
    public class VcLayout: ViewComponent
    {
        private readonly AppDbContext _context;

        public VcLayout(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            VmLayout VmLayout = new VmLayout()
            {
                Setting = _context.Settings.FirstOrDefault(),
                Socials = _context.Socials.ToList()
            };
            return View(VmLayout);
        }
    }
}

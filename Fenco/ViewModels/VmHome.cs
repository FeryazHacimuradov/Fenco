using Fenco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.ViewModels
{
    public class VmHome : VmLayout
    {
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Service> Services { get; set; }

    }
}

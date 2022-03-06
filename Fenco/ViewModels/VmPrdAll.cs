using Fenco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.ViewModels
{
    public class VmPrdAll
    {
        public Product Product { get; set; }
        public List<VmColorImage> ColorImages { get; set; }
    }
}

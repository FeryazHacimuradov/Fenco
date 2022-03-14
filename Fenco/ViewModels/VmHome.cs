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
        public List<Category> Categories { get; set; }
        public List<Portfolio> Portfolios { get; set; }
        public Blog Blog { get; set; }
        public Portfolio Portfolio { get; set; }
        public List<Tag> Tags { get; set; }
        public int? prId { get; set; }
    }
}

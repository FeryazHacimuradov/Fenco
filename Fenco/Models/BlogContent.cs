using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Models
{
    public class BlogContent
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nText"), Required]
        public string Content { get; set; }
        public List<Blog> Blogs { get; set; }

    }
}

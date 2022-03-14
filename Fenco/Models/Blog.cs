using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500), Required]
        public string Title { get; set; }
        [ForeignKey("BlogImage"), Required]
        public int BlogImageId { get; set; }
        public BlogImage BlogImage { get; set; }
        [ForeignKey("BlogContent"), Required]
        public int BlogContentId { get; set; }
        public BlogContent BlogContent { get; set; }
        [ForeignKey("Category"), Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("CustomUser")]
        public string CustomUserId { get; set; }
        [NotMapped]
        public CustomUser CustomUser { get; set; }

    }
}

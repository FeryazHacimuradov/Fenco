using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Models
{
    public class Portfolio
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500), Required]
        public string Title { get; set; }
        [ForeignKey("PortfolioImage"), Required]
        public int PortfolioImageId { get; set; }
        public PortfolioImage PortfolioImage { get; set; }
        [Column(TypeName = "nText"), Required]
        public string Content { get; set; }
        [ForeignKey("Category"), Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public int[] TagsId { get; set; }
        public List<TagToPortfolio> TagToPortfolios { get; set; }
    }
}

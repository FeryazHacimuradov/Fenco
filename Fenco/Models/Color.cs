﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.Models
{
    public class Color
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50), Required]
        public string Name { get; set; }
        [ForeignKey("ParentColor")]
        public int? ParentColorId { get; set; }
        public Color ParentColor { get; set; }
        public List<ColorToProduct> ColorToProducts { get; set; }
    }
}

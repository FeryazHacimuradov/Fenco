using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.ViewModels
{
    public class VmRegister
    {
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Surname { get; set; }

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [MaxLength(30)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
    }
}

﻿using Fenco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fenco.ViewModels
{
    public class VmUser
    {
        public List<CustomUser> CustomUsers { get; set; }
        public Dictionary<string, string> UserRoles { get; set; }

    }
}

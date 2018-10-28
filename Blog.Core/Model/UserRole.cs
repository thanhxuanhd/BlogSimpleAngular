﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Core.Model
{
    public class UserRole : IdentityRole<Guid>
    {
        [MaxLength(256)]
        public string Description { get; set; }

        public IList<IdentityUserRole<Guid>> AppUserRoles { get; } = new List<IdentityUserRole<Guid>>();
    }
}

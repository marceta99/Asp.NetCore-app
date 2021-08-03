﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vroom.Models
{
    public class VroomDbContext : IdentityDbContext<IdentityUser>
    {

        public VroomDbContext(DbContextOptions options ) : base(options)
        {

        }

        public DbSet<Make> Makes { get; set;  }
        public DbSet<Model> Models { get; set;  }
        public DbSet<ApplicationUser> ApplicationUsers { get; set;  }

        public DbSet<Bike> Bikes { get; set; }
    }
}

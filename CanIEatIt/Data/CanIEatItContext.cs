﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CanIEatIt.Models;

namespace CanIEatIt.Data
{
    public class CanIEatItContext : DbContext
    {
        public CanIEatItContext (DbContextOptions<CanIEatItContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CanIEatIt.Models.Mushroom> Mushroom { get; set; } = default!;
        public virtual DbSet<CanIEatIt.Models.Plant>? Plant { get; set; }
    }
}

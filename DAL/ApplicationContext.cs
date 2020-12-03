﻿using Entities.Entities.UserAndSecurity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class ApplicationContext: IdentityDbContext<Users>
    {

        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }
    }
}

using LPA.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DataLayer.Mapping
{
    public static class CustomMappings
    {
        public static void AddCustomMappings(this ModelBuilder modelBuilder, ApiSettings siteSettings)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomMappings).Assembly);
        }
    }
}

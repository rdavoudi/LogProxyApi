using LPA.Model.ThirdParty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DataLayer.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> build)
        {
            build.Property(x => x.Title).HasMaxLength(450).IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTimeDataAccess.Models;

namespace ShowTimeDataAccess.Configurations;

public class FestivalConfiguration : IEntityTypeConfiguration<Festivals>
{
    public void Configure(EntityTypeBuilder<Festivals> builder)
    {
        builder.ToTable("Festival");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(a => a.StartDate < a.EndDate)
            .IsRequired();
    }
}


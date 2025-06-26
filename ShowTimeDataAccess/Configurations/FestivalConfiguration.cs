using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.DataAccess.Models;

namespace ShowTime.DataAccess.Configurations;

public class FestivalConfiguration : IEntityTypeConfiguration<Festival>
{
    public void Configure(EntityTypeBuilder<Festival> builder)
    {
        builder.ToTable("Festival");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.StartDate)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(a => a.EndDate)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(a => a.Location)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.SplashArt)  
            .IsRequired()
            .HasMaxLength(200); 

        builder.Property(a => a.Capacity)   
            .IsRequired()
            .HasDefaultValue(0);


        builder.HasMany(f => f.Artists)
            .WithMany(a => a.Festivals)
            .UsingEntity<Lineup>();

        builder.HasMany(f => f.Lineups)
            .WithOne(l => l.Festival)
            .HasForeignKey(l => l.FestivalId);

        builder.HasMany(f => f.Bookings)
            .WithOne(b => b.Festival)
            .HasForeignKey(b => b.FestivalId);

    }
}


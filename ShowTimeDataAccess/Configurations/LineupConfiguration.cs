using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTimeDataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShowTimeDataAccess.Configurations;

public class LineupConfiguration : IEntityTypeConfiguration<Models.Lineup>
{
    public void Configure(EntityTypeBuilder<Lineup> builder)
    {
        builder.ToTable("Lineup");
        builder.HasKey(l => new { l.FestivalId, l.ArtistId });
        builder.Property(l => l.Stage)
            .IsRequired()
            .HasMaxLength(100);
    }
}
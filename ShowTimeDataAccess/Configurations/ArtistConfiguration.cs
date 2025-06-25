using Microsoft.EntityFrameworkCore;
using ShowTimeDataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTimeDataAccess.Models;


namespace ShowTimeDataAccess.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(a => a.Festivals)
            .WithMany(f => f.Artists)
            .UsingEntity<Lineup>();
    }
}

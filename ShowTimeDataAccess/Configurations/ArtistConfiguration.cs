using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShowTime.DataAccess.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Genre)
            .IsRequired();

        builder.Property(a => a.Image)
            .IsRequired();
    }
}

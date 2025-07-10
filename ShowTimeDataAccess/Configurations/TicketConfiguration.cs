using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShowTime.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Price)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(t => t.TicketsTotal)
            .IsRequired();

        builder.Property(t => t.TicketType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.FestivalId)
            .IsRequired();

        builder.Property(t => t.IsAvailable)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(t => t.Festival)
            .WithMany(f => f.Tickets)
            .HasForeignKey(t => t.FestivalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Bookings)
            .WithOne(b => b.Ticket)
            .HasForeignKey(b => b.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShowTime.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Configurations;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.TicketId)
            .IsRequired();

        builder.Property(b => b.BookingStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);


        builder.HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Ticket)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}



using Microsoft.EntityFrameworkCore;
using ShowTime.DataAccess.Configurations;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess;

public class ShowTimeDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ShowTimeDbContext(DbContextOptions<ShowTimeDbContext> options) : base(options)
    {
    }
    public DbSet<Festival> Festivals { get; set; } = null!;
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Lineup> Lineups { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new FestivalConfiguration());
        modelBuilder.ApplyConfiguration(new ArtistConfiguration());
        modelBuilder.ApplyConfiguration(new LineupConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());



        modelBuilder.Entity<Artist>().HasData(new Artist
        {
            Id = 2,
            Name = "Lady Gaga",
            Image = "",
            Genre = "Pop"
        });

        modelBuilder.Entity<Artist>().HasData(new Artist
        {
            Id = 3,
            Name = "Måneskin",
            Image = "",
            Genre = "Rock"
        });

        modelBuilder.Entity<Artist>().HasData(new Artist
        {
            Id = 1,
            Name = "Dua Lipa",
            Image = "https://cdn.dc5.ro/img-prod/1981989-0.jpeg",
            Genre = "Pop",
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 1,
            Name = "Glastonbury",
            StartDate = new DateTime(2025, 7, 26),
            EndDate = new DateTime(2025, 7, 29),
            Location = "England",
            SplashArt = "images/Glastologo.png",
            Capacity = 200000
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 2,
            Name = "ElectricCastle",
            StartDate = new DateTime(2025, 9, 8),
            EndDate = new DateTime(2025, 9, 12),
            Location = "Bontida",
            SplashArt = "images/ec.jpg",
            Capacity = 200000
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 3,
            Name = "Untold",
            StartDate = new DateTime(2025, 8, 1),
            EndDate = new DateTime(2025, 8, 5),
            Location = "ClujNapoca",
            SplashArt = "images/untold.png",
            Capacity = 400000
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 4,
            Name = "NovaRock",
            StartDate = new DateTime(2025, 7, 26),
            EndDate = new DateTime(2025, 7, 29),
            Location = "Austria",
            SplashArt = "images/novalogo.png",
            Capacity = 50000
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 5,
            Name = "Coachella",
            StartDate = new DateTime(2026, 4, 19),
            EndDate = new DateTime(2026, 4, 28),
            Location = "England",
            SplashArt = "images/coachella.png",
            Capacity = 800000
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 6,
            Name = "Sziget",
            StartDate = new DateTime(2026, 5, 10),
            EndDate = new DateTime(2026, 5, 16),
            Location = "Budapest",
            SplashArt = "images/sziget.jpg",
            Capacity = 200000
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 7,
            Name = "Tomorrowland",
            StartDate = new DateTime(2025, 10, 10),
            EndDate = new DateTime(2025, 10, 20),
            Location = "Belgium",
            SplashArt = "images/tmrwl.png",
            Capacity = 500000
        });

        modelBuilder.Entity<Festival>().HasData(new Festival
        {
            Id = 8,
            Name = "SummerWell",
            StartDate = new DateTime(2025, 8, 10),
            EndDate = new DateTime(2025, 8, 11),
            Location = "Bucharest",
            SplashArt = "images/summerwell.png",
            Capacity = 500000
        });


    }
}

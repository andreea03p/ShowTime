
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTimeDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using ShowTimeDataAccess.Configurations;

namespace ShowTimeDataAccess;

public class ShowTimeDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ShowTimeDbContext(DbContextOptions<ShowTimeDbContext> options) : base(options)
    {
    }
    public DbSet<Festivals> Festivals { get; set; } = null!;
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Lineup> Lineups { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShowTimeDbContext).Assembly);
    }
}

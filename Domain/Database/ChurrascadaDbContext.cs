using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database;

public class ChurrascadaDbContext : DbContext
{
    public ChurrascadaDbContext(DbContextOptions<ChurrascadaDbContext> options)
        : base(options) { }
    
    public DbSet<Agenda> Agendas { get; set; }
    public DbSet<Churras> Churras { get; set; }
    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agenda>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        
        modelBuilder.Entity<Churras>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasBaseType<Agenda>();
        });
        
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.OwnsMany<Agenda>(e => e.Agendas);
        });
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
}
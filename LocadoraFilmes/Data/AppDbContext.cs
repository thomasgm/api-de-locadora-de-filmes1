using Microsoft.EntityFrameworkCore;
using LocadoraFilmes.Models;

namespace LocadoraFilmes.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Aluguel> Alugueis { get; set; }
    public DbSet<Genero> Generos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluguel>(entity=>
        {
            entity.HasKey(a => a.Id);
            entity.HasOne(a => a.Filme)
                .WithMany()
                .HasForeignKey(a => a.FilmeId)
                .OnDelete(DeleteBehavior.Restrict);//impede deletar filmes alugados
            
            entity.HasOne(a => a.Cliente)
                .WithMany()
                .HasForeignKey(a => a.ClienteId);
        });

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Filme>()
            .HasMany(f => f.Generos)
            .WithMany(g => g.Filmes)
            .UsingEntity(j => j.ToTable("FilmeGenero"));
        
        base.OnModelCreating(modelBuilder);
    }
}
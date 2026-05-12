using Microsoft.EntityFrameworkCore;
using LocadoraFilmes.Models;

namespace LocadoraFilmes.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Filme> Filmes { get; set; }
}
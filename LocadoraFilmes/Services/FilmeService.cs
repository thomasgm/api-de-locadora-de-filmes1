using LocadoraFilmes.Data;
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraFilmes.Services;

public class FilmeService : IFilmeService
{
    private readonly AppDbContext _context;

    public FilmeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FilmeResponseDto>> GetAllAsync(int page, int pageSize)
    {
        return await _context.Filmes
            .Include(f => f.Generos) // ✅ carrega os gêneros
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(f => ToResponseDto(f))
            .ToListAsync();
    }

    public async Task<FilmeResponseDto?> GetByIdAsync(int id)
    {
        var filme = await _context.Filmes
            .Include(f => f.Generos) // ✅ carrega os gêneros
            .FirstOrDefaultAsync(f => f.Id == id);

        return filme is null ? null : ToResponseDto(filme);
    }

    public async Task<FilmeResponseDto> CreateAsync(FilmeCreateDto dto)
    {
        var filme = new Filme
        {
            Titulo = dto.Titulo,
            Diretor = dto.Diretor,
            Ano = dto.Ano,
            QuantidadeDisponivel = dto.QuantidadeDisponivel
        };

        foreach (var nomeGenero in dto.Generos)
        {
            var generoExistente = await _context.Generos
                .FirstOrDefaultAsync(g => g.Nome.ToLower() == nomeGenero.ToLower());

            filme.Generos.Add(generoExistente ?? new Genero { Nome = nomeGenero });
        }

        _context.Filmes.Add(filme);
        await _context.SaveChangesAsync();

        return ToResponseDto(filme);
    }

    public async Task<FilmeResponseDto?> UpdateAsync(int id, FilmeUpdateDto dto)
    {
        var filme = await _context.Filmes
            .Include(f => f.Generos)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (filme is null) return null;

        filme.Titulo = dto.Titulo;
        filme.Diretor = dto.Diretor;
        filme.Ano = dto.Ano;
        filme.QuantidadeDisponivel = dto.QuantidadeDisponivel;

        // Remove gêneros que não vieram no DTO
        var generosParaRemover = filme.Generos
            .Where(g => !dto.Generos.Select(n => n.ToLower()).Contains(g.Nome.ToLower()))
            .ToList();

        foreach (var genero in generosParaRemover)
            filme.Generos.Remove(genero);

        // Adiciona gêneros novos
        foreach (var nomeGenero in dto.Generos)
        {
            if (filme.Generos.Any(g => g.Nome.ToLower() == nomeGenero.ToLower()))
                continue;

            var generoBanco = await _context.Generos
                .FirstOrDefaultAsync(g => g.Nome.ToLower() == nomeGenero.ToLower());

            filme.Generos.Add(generoBanco ?? new Genero { Nome = nomeGenero });
        }

        await _context.SaveChangesAsync();

        return ToResponseDto(filme);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var filme = await _context.Filmes.FindAsync(id);
        if (filme is null) return false;

        _context.Filmes.Remove(filme);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<FilmeResponseDto>> SearchByNameAsync(string nome)
    {
        return await _context.Filmes
            .Include(f => f.Generos) // ✅ carrega os gêneros
            .Where(f => f.Titulo.Contains(nome))
            .Select(f => ToResponseDto(f))
            .ToListAsync();
    }

    private static FilmeResponseDto ToResponseDto(Filme f) => new(
        f.Id,
        f.Titulo,
        f.Diretor,
        f.Ano,
        f.Generos.Select(g => new GeneroDTO(g.Id, g.Nome)),
        f.QuantidadeDisponivel,
        f.AdicionadoEm
    );
}
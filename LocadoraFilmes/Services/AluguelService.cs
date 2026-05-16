/*
1. Busca o cliente pelo clienteId — se não existir retorna null
2. Busca o filme pelo filmeId — se não existir retorna null
3. Verifica se QuantidadeDisponivel > 0 — se não retorna null
4. Cria o Aluguel no banco
5. Diminui o estoque do filme
6. Retorna o AluguelResponseDto
*/

using LocadoraFilmes.Data;
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraFilmes.Services;
public class AluguelService : IAluguelService
{
    private readonly AppDbContext _context;

    public AluguelService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AluguelResponseDto?> AlugarFilmeAsync(int clienteId, int filmeId)
    {
        var cliente = await _context.Clientes.FindAsync(clienteId);
        if (cliente is null) return null;

        var filme = await _context.Filmes.FindAsync(filmeId);
        if (filme is null || filme.QuantidadeDisponivel <= 0) return null;

        var aluguel = new Aluguel
        {
            ClienteId = clienteId,
            FilmeId = filmeId,
            DataAluguel = DateTime.UtcNow,
            Devolvido = false
        };

        _context.Alugueis.Add(aluguel);
        filme.QuantidadeDisponivel--;
        await _context.SaveChangesAsync();

        return new AluguelResponseDto(
            aluguel.Id,
            filme.Titulo,
            aluguel.DataAluguel,
            aluguel.DataDevolucao,
            aluguel.Devolvido
        );
    }
    /*
    DevolverFilmeAsync:

Busca o aluguel pelo id — se não existir retorna false
Se já foi devolvido retorna false
Marca como devolvido e preenche DataDevolucao
Aumenta o estoque do filme
Salva e retorna true
*/
    public async Task<bool> DevolverFilmeAsync(int aluguelId)
    {
        var aluguel = await _context.Alugueis
            .Include(a => a.Filme)
            .FirstOrDefaultAsync(a => a.Id == aluguelId);

        if (aluguel is null || aluguel.Devolvido) return false;

        aluguel.Devolvido = true;
        aluguel.DataDevolucao = DateTime.UtcNow;
        aluguel.Filme!.QuantidadeDisponivel++;

        await _context.SaveChangesAsync();
        return true;
    }
/*

ObterAlugueisPorClienteAsync:

Busca todos os aluguéis do cliente
Retorna a lista como AluguelResponseDto
*/
    public async Task<IEnumerable<AluguelResponseDto>> ObterAlugueisPorClienteAsync(int clienteId)
    {
        return await _context.Alugueis
            .Where(a => a.ClienteId == clienteId)
            .Include(a => a.Filme)
            .Select(a => new AluguelResponseDto(
                a.Id,
                a.Filme!.Titulo,
                a.DataAluguel,
                a.DataDevolucao,
                a.Devolvido
            ))
            .ToListAsync();
    }
}
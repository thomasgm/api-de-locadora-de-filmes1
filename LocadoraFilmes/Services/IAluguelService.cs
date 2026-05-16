using LocadoraFilmes.DTOs;

namespace LocadoraFilmes.Services;

public interface IAluguelService
{
    Task<AluguelResponseDto?> AlugarFilmeAsync(int clienteId, int filmeId);
    Task<bool> DevolverFilmeAsync(int aluguelId);
    Task<IEnumerable<AluguelResponseDto>> ObterAlugueisPorClienteAsync(int clienteId);
}
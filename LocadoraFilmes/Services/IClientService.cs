using LocadoraFilmes.DTOs;

namespace LocadoraFilmes.Services;

public interface IClienteService
{
    Task<string?> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}
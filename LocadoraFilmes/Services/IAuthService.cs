using LocadoraFilmes.DTOs;

namespace LocadoraFilmes.Services;

public interface IAuthService
{
    Task<string?> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}
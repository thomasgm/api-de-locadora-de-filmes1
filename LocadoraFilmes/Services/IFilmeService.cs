using LocadoraFilmes.DTOs;

namespace LocadoraFilmes.Services;

public interface IFilmeService
{
    Task<IEnumerable<FilmeResponseDto>> GetAllAsync(int page, int pageSize);
    Task<FilmeResponseDto?> GetByIdAsync(int id);
    Task<FilmeResponseDto> CreateAsync(FilmeCreateDto dto);
    Task<FilmeResponseDto?> UpdateAsync(int id, FilmeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FilmeResponseDto>> SearchByNameAsync(string nome);
}
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraFilmes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmesController : ControllerBase
{
    private readonly IFilmeService _filmeService;

    public FilmesController(IFilmeService filmeService)
    {
        _filmeService = filmeService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] string? titulo, [FromQuery] string? genero, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var filmes = await _filmeService.GetAllAsync(page, pageSize);
        return Ok(filmes);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var filme = await _filmeService.GetByIdAsync(id);
        if (filme is null) return NotFound(new { message = "Filme não encontrado." });
        return Ok(filme);
    }

    [HttpGet("buscar")]
    [AllowAnonymous]
    public async Task<IActionResult> Search([FromQuery] string titulo)
    {
        var filmes = await _filmeService.SearchByNameAsync(titulo);
        return Ok(filmes);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] FilmeCreateDto dto)
    {
        var criado = await _filmeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] FilmeUpdateDto dto)
    {
        var atualizado = await _filmeService.UpdateAsync(id, dto);
        if (atualizado is null) return NotFound(new { message = "Filme não encontrado." });
        return Ok(atualizado);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _filmeService.DeleteAsync(id);
        if (!deletado) return NotFound(new { message = "Filme não encontrado." });
        return NoContent();
    }
}
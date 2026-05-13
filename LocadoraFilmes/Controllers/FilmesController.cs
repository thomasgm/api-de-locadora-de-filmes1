using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LocadoraFilmes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmesController : ControllerBase
{
    private readonly IFilmeService _service;

    public FilmesController(IFilmeService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var filmes = await _service.GetAllAsync(page, pageSize);
        return Ok(filmes);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var filme = await _service.GetByIdAsync(id);
        if (filme is null) return NotFound();
        return Ok(filme);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] FilmeCreateDto dto)
    {
        var criado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] FilmeUpdateDto dto)
    {
        var atualizado = await _service.UpdateAsync(id, dto);
        if (atualizado is null) return NotFound();
        return Ok(atualizado);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _service.DeleteAsync(id);
        if (!deletado) return NotFound();
        return NoContent();
    }
}
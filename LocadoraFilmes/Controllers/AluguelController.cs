using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraFilmes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AluguelController : ControllerBase
{
    private readonly IAluguelService _aluguelService;

    public AluguelController(IAluguelService aluguelService)
    {
        _aluguelService = aluguelService;
    }

    [HttpPost("alugar")]
    [Authorize]
    public async Task<IActionResult> AlugarFilme([FromQuery] int clienteId, [FromQuery] int filmeId)
    {
        var resultado = await _aluguelService.AlugarFilmeAsync(clienteId, filmeId);
        if (resultado is null) return BadRequest(new { message = "Não foi possível alugar o filme. Verifique se o cliente e o filme existem e se o filme está disponível." });
        return Ok(resultado);
    }

    [HttpPut("devolver")]
    [Authorize]
    public async Task<IActionResult> DevolverFilme([FromQuery] int aluguelId)
    {
        var resultado = await _aluguelService.DevolverFilmeAsync(aluguelId);
        if (!resultado) return BadRequest(new { message = "Não foi possível devolver o filme. Verifique se o aluguel existe e se o filme já foi devolvido." });
        return Ok(new { message = "Filme devolvido com sucesso." });
    }

    [HttpGet("cliente/{clienteId}")]
    [Authorize]
    public async Task<IActionResult> ObterAlugueisPorCliente(int clienteId)
    {
        var alugueis = await _aluguelService.ObterAlugueisPorClienteAsync(clienteId);
        return Ok(alugueis);
    }
}
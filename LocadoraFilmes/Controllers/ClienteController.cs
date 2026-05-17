using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocadoraFilmes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var token = await _clienteService.RegisterAsync(dto);
        if (token is null) return Conflict("Email já cadastrado.");
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _clienteService.LoginAsync(dto);
        if (token is null) return Unauthorized("Email ou senha inválidos.");
        return Ok(new { token });
    }
}
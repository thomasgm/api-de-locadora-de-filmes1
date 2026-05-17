using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LocadoraFilmes.Data;
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LocadoraFilmes.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public ClienteService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string?> RegisterAsync(RegisterDto dto)
    {
        var existe = await _context.Clientes.AnyAsync(u => u.Email == dto.Email);
        if (existe) return null;

        var cliente = new Cliente
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return GerarToken(cliente);
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (cliente is null) return null;

        var senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, cliente.SenhaHash);
        if (!senhaValida) return null;

        return GerarToken(cliente);
    }

    private string GerarToken(Cliente cliente)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, cliente.Id.ToString()),
            new Claim(ClaimTypes.Name, cliente.Nome),
            new Claim(ClaimTypes.Email, cliente.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
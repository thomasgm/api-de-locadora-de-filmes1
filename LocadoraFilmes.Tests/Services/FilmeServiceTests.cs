using LocadoraFilmes.Data;
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace LocadoraFilmes.Tests.Services;

public class FilmeServiceTests
{
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarFilme_QuandoDadosValidos()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);
        var dto = new FilmeCreateDto
        {
            Titulo = "Teste",
            Diretor = "Diretor Teste",
            Ano = 2020,
            QuantidadeDisponivel = 5,
            Generos = new List<string> { "Ação", "Comédia" }
        };

        // Act
        var resultado = await service.CreateAsync(dto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(dto.Titulo, resultado.Titulo);
        Assert.Equal(dto.Diretor, resultado.Diretor);
        Assert.Equal(dto.Ano, resultado.Ano);
        Assert.Equal(dto.QuantidadeDisponivel, resultado.QuantidadeDisponivel);
        Assert.Equal(dto.Generos.Count, resultado.Generos.Count());
    }
}
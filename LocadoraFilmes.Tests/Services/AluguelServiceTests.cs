using LocadoraFilmes.Data;
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using LocadoraFilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraFilmes.Tests.Services;

public class AluguelServiceTests
{
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AlugarFilmeAsync_DeveRetornarAluguel_QuandoTudoEstaCorreto()
    {
        // Arrange
        var contexto = CriarContexto();
        var service = new AluguelService(contexto);

        var cliente = new Cliente { Nome = "João" };
        var filme = new Filme { Titulo = "Matrix", QuantidadeDisponivel = 5 };

        contexto.Clientes.Add(cliente);
        contexto.Filmes.Add(filme);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.AlugarFilmeAsync(cliente.Id, filme.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Matrix", resultado!.Titulo);
        Assert.False(resultado.Devolvido);
    }

    [Fact]
    public async Task AlugarFilmeAsync_DeveRetornarNull_QuandoFilmeIndisponivel()
    {
        // Arrange
        var contexto = CriarContexto();
        var service = new AluguelService(contexto);

        var cliente = new Cliente { Nome = "Maria" };
        var filme = new Filme { Titulo = "Inception", QuantidadeDisponivel = 0 };

        contexto.Clientes.Add(cliente);
        contexto.Filmes.Add(filme);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.AlugarFilmeAsync(cliente.Id, filme.Id);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task AlugarFilmeAsync_DeveRetornarNull_QuandoClienteNaoExiste()
    {
        // Arrange
        var contexto = CriarContexto();
        var service = new AluguelService(contexto);

        var filme = new Filme { Titulo = "Avatar", QuantidadeDisponivel = 3 };
        contexto.Filmes.Add(filme);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.AlugarFilmeAsync(999, filme.Id); // Cliente inexistente

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task AlugarFilmeAsync_DeveRetornarNull_QuandoFilmeNaoExiste()
    {
        // Arrange
        var contexto = CriarContexto();
        var service = new AluguelService(contexto);

        var cliente = new Cliente { Nome = "Carlos" };
        contexto.Clientes.Add(cliente);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.AlugarFilmeAsync(cliente.Id, 999); // Filme inexistente

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task DevolverFilmeAsync_DeveRetornarTrue_QuandoDevolucaoBemSucedida()
    {
        // Arrange
        var contexto = CriarContexto();
        var service = new AluguelService(contexto);

        var cliente = new Cliente { Nome = "Ana" };
        var filme = new Filme { Titulo = "Gladiator", QuantidadeDisponivel = 2 };

        contexto.Clientes.Add(cliente);
        contexto.Filmes.Add(filme);
        await contexto.SaveChangesAsync();

        var aluguel = new Aluguel
        {
            ClienteId = cliente.Id,
            FilmeId = filme.Id,
            DataAluguel = DateTime.Now,
            Devolvido = false
        };
        contexto.Alugueis.Add(aluguel);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.DevolverFilmeAsync(aluguel.Id);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public async Task DevolverFilmeAsync_DeveRetornarFalse_QuandoDevolucaoFalhar()
    {
        // Arrange
        var contexto = CriarContexto();
        var service = new AluguelService(contexto);

        // Act
        var resultado = await service.DevolverFilmeAsync(999); // Aluguel inexistente

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public async Task DevolverFilmeAsync_DeveRetornarFalse_QuandoFilmeJaDevolvido()
    {
        // Arrange
        var contexto = CriarContexto();
        var service = new AluguelService(contexto);

        var cliente = new Cliente { Nome = "Pedro" };
        var filme = new Filme { Titulo = "Titanic", QuantidadeDisponivel = 1 };

        contexto.Clientes.Add(cliente);
        contexto.Filmes.Add(filme);
        await contexto.SaveChangesAsync();

        var aluguel = new Aluguel
        {
            ClienteId = cliente.Id,
            FilmeId = filme.Id,
            DataAluguel = DateTime.Now,
            Devolvido = true // Já devolvido
        };
        contexto.Alugueis.Add(aluguel);
        await contexto.SaveChangesAsync();

        // Act
        var resultado = await service.DevolverFilmeAsync(aluguel.Id);

        // Assert
        Assert.False(resultado);
    }
}
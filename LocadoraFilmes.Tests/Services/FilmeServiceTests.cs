using LocadoraFilmes.Data;
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using LocadoraFilmes.Models;
using Microsoft.EntityFrameworkCore;

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

    [Fact]
    public async Task GetByIdAsync_DeveRetornarFilme_QuandoExiste() {
        //Arrange
        // prepara o banco com um filme de teste
        var context = CriarContexto();
        var service = new FilmeService(context);

        context.Filmes.Add(new Filme
        {
            Id = 1,
            Titulo = "Teste",
            Diretor = "Diretor Teste",
            Ano = 2020,
            QuantidadeDisponivel = 5,
            AdicionadoEm = DateTime.Now
        });
        await context.SaveChangesAsync();

        //Act - busca o filme que acabamos de adicionar
        var resultado = await service.GetByIdAsync(1);

        //Assert - verifica se o resultado é válido e tem os dados corretos
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal("Teste", resultado.Titulo);
        Assert.Equal("Diretor Teste", resultado.Diretor);
        Assert.Equal(2020, resultado.Ano);
        Assert.Equal(5, resultado.QuantidadeDisponivel);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste() {
        //Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);

        //Act - tenta buscar um filme que não existe
        var resultado = await service.GetByIdAsync(999);

        //Assert - verifica se o resultado é null
        Assert.Null(resultado);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarFilmesPaginados()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);

        // Adiciona 15 filmes de teste
        for (int i = 1; i <= 15; i++)
        {
            context.Filmes.Add(new Filme
            {
                Id = i,
                Titulo = $"Filme {i}",
                Diretor = $"Diretor {i}",
                Ano = 2000 + i,
                QuantidadeDisponivel = i,
                AdicionadoEm = DateTime.Now
            });
        }
        await context.SaveChangesAsync();

        // Act - busca a primeira página com 10 itens
        var resultado = await service.GetAllAsync(1, 10);

        // Act - busca a SEGUNDA página
        var resultadoPagina2 = await service.GetAllAsync(2, 10);

        // Assert - verifica se o resultado tem os dados corretos
        Assert.NotNull(resultado);
        Assert.Equal(10, resultado.Count());
        Assert.Equal("Filme 1", resultado.First().Titulo);
        Assert.Equal("Filme 10", resultado.Last().Titulo);

        //Assert pagina 2 - verifica se o resultado tem os dados corretos
        Assert.Equal(5, resultadoPagina2.Count()); // sobram 5 filmes
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoBancoVazio()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);

        // Act - busca a primeira página com 10 itens
        var resultado = await service.GetAllAsync(1, 10);

        // Assert - verifica se o resultado é uma lista vazia
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarFilme_QuandoDadosValidos()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);
    
        // prepara o banco com um filme de teste
        context.Filmes.Add(new Filme
        {
            Id = 1,
            Titulo = "Teste",
            Diretor = "Diretor Teste",
            Ano = 2020,
            QuantidadeDisponivel = 5,
            AdicionadoEm = DateTime.Now
        });
        await context.SaveChangesAsync();
        var dto = new FilmeUpdateDto
        {
            Titulo = "Teste Atualizado",
            Diretor = "Diretor Atualizado",
            Ano = 2021,
            QuantidadeDisponivel = 10,
            Generos = new List<string> { "Ação", "Drama" }
        };
        
        // Act - atualiza o filme que acabamos de adicionar

        var resultado = await service.UpdateAsync(1, dto);

        // Assert - verifica se o resultado é válido e tem os dados atualizados
        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal(dto.Titulo, resultado.Titulo);
        Assert.Equal(dto.Diretor, resultado.Diretor);
        Assert.Equal(dto.Ano, resultado.Ano);
        Assert.Equal(dto.Generos, resultado.Generos.Select(g => g.Nome));
        Assert.Equal(dto.QuantidadeDisponivel, resultado.QuantidadeDisponivel);
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarNull_QuandoFilmeNaoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);
        var dto = new FilmeUpdateDto
        {
            Titulo = "Teste Atualizado",
            Diretor = "Diretor Atualizado",
            Ano = 2021,
            QuantidadeDisponivel = 10,
            Generos = new List<string> { "Ação", "Drama" }
        };

        // Act - tenta atualizar um filme que não existe
        var resultado = await service.UpdateAsync(999, dto);

        // Assert - verifica se o resultado é null
        Assert.Null(resultado);
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverFilme_QuandoFilmeExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);

        // prepara o banco com um filme de teste
        context.Filmes.Add(new Filme
        {
            Id = 1,
            Titulo = "Teste",
            Diretor = "Diretor Teste",
            Ano = 2020,
            QuantidadeDisponivel = 5,
            AdicionadoEm = DateTime.Now
        });
        await context.SaveChangesAsync();

        // Act - remove o filme que acabamos de adicionar
        var resultado = await service.DeleteAsync(1);

        // Assert - verifica se o resultado é true e o filme foi removido do banco
        Assert.True(resultado);
        Assert.Null(await service.GetByIdAsync(1));
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarFalse_QuandoFilmeNaoExiste()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);

        // Act - tenta remover um filme que não existe
        var resultado = await service.DeleteAsync(999);

        // Assert - verifica se o resultado é false
        Assert.False(resultado);
    }

    [Fact]
    public async Task SearchByNameAsync_DeveRetornarFilmes_QuandoExistemFilmesComNome()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);

        // Adiciona filmes de teste
        context.Filmes.Add(new Filme
        {
            Id = 1,
            Titulo = "Teste",
            Diretor = "Diretor Teste",
            Ano = 2020,
            QuantidadeDisponivel = 5,
            AdicionadoEm = DateTime.Now
        });
        context.Filmes.Add(new Filme
        {
            Id = 2,
            Titulo = "Outro Filme",
            Diretor = "Outro Diretor",
            Ano = 2021,
            QuantidadeDisponivel = 3,
            AdicionadoEm = DateTime.Now
        });
        await context.SaveChangesAsync();

        // Act - busca filmes com nome "Teste"
        var resultado = await service.SearchByNameAsync("Teste");

        // Assert - verifica se o resultado tem os dados corretos
        Assert.NotNull(resultado);
        Assert.Single(resultado);
        Assert.Equal("Teste", resultado.First().Titulo);
    }

    [Fact]
    public async Task SearchByNameAsync_DeveRetornarListaVazia_QuandoNaoExistemFilmesComNome()
    {
        // Arrange
        var context = CriarContexto();
        var service = new FilmeService(context);

        // Adiciona filmes de teste
        context.Filmes.Add(new Filme
        {
            Id = 1,
            Titulo = "Teste",
            Diretor = "Diretor Teste",
            Ano = 2020,
            QuantidadeDisponivel = 5,
            AdicionadoEm = DateTime.Now
        });
        await context.SaveChangesAsync();

        // Act - busca filmes com nome "Inexistente"
        var resultado = await service.SearchByNameAsync("Inexistente");

        // Assert - verifica se o resultado é uma lista vazia
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }
    
}
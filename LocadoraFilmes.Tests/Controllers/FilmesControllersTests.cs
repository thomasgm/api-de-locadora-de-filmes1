using LocadoraFilmes.Controllers;
using LocadoraFilmes.DTOs;
using LocadoraFilmes.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LocadoraFilmes.Tests.Controllers;

public class FilmesControllerTests
{
    [Fact]
    public async Task GetAll_DeveRetornarOk_ComListaDeFilmes()
    {
        // Arrange
        var filmes = new List<FilmeResponseDto>
        {
            new FilmeResponseDto
            {
                Id = 1,
                Titulo = "Matrix"
            },
            new FilmeResponseDto
            {
                Id = 2,
                Titulo = "Interestelar"
            }
        };

        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.GetAllAsync(1, 10))
            .ReturnsAsync(filmes);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var retorno = Assert.IsAssignableFrom<IEnumerable<FilmeResponseDto>>(okResult.Value);

        Assert.Equal(2, retorno.Count());
    }

    [Fact]
    public async Task GetById_DeveRetornarOk_QuandoFilmeExistir()
    {
        // Arrange
        var filme = new FilmeResponseDto
        {
            Id = 1,
            Titulo = "Matrix"
        };

        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(filme);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var retorno = Assert.IsType<FilmeResponseDto>(okResult.Value);

        Assert.Equal("Matrix", retorno.Titulo);
    }

    [Fact]
    public async Task GetById_DeveRetornarNotFound_QuandoFilmeNaoExistir()
    {
        // Arrange
        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync((FilmeResponseDto?)null);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.GetById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_DeveRetornarCreatedAtAction_QuandoFilmeForCriado()
    {
        // Arrange
        var dto = new FilmeCreateDto
        {
            Titulo = "Batman Begins",
            Diretor = "Christopher Nolan",
            Ano = 2005,
            QuantidadeDisponivel = 5
        };

        var filmeCriado = new FilmeResponseDto
        {
            Id = 1,
            Titulo = dto.Titulo,
            Diretor = dto.Diretor,
            Ano = dto.Ano,
            QuantidadeDisponivel = dto.QuantidadeDisponivel
        };

        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(filmeCriado);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        var retorno = Assert.IsType<FilmeResponseDto>(createdResult.Value);

        Assert.Equal("Batman Begins", retorno.Titulo);

        Assert.Equal(nameof(controller.GetById), createdResult.ActionName);
    }

    [Fact]
    public async Task Update_DeveRetornarOk_QuandoFilmeForAtualizado()
    {
        // Arrange
        var dto = new FilmeUpdateDto
        {
            Titulo = "Matrix Reloaded",
            Diretor = "Wachowski",
            Ano = 2003,
            QuantidadeDisponivel = 10
        };

        var filmeAtualizado = new FilmeResponseDto
        {
            Id = 1,
            Titulo = dto.Titulo,
            Diretor = dto.Diretor,
            Ano = dto.Ano,
            QuantidadeDisponivel = dto.QuantidadeDisponivel
        };

        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.UpdateAsync(1, dto))
            .ReturnsAsync(filmeAtualizado);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.Update(1, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var retorno = Assert.IsType<FilmeResponseDto>(okResult.Value);

        Assert.Equal("Matrix Reloaded", retorno.Titulo);
    }

    [Fact]
    public async Task Update_DeveRetornarNotFound_QuandoFilmeNaoExistir()
    {
        // Arrange
        var dto = new FilmeUpdateDto
        {
            Titulo = "Filme Inexistente",
            Diretor = "Diretor",
            Ano = 2020,
            QuantidadeDisponivel = 1
        };

        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.UpdateAsync(1, dto))
            .ReturnsAsync((FilmeResponseDto?)null);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.Update(1, dto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_DeveRetornarNoContent_QuandoFilmeForRemovido()
    {
        // Arrange
        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(true);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_DeveRetornarNotFound_QuandoFilmeNaoExistir()
    {
        // Arrange
        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(false);

        var controller = new FilmesController(mockService.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAll_DeveChamarServiceCorretamente()
    {
        // Arrange
        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.GetAllAsync(1, 10))
            .ReturnsAsync(new List<FilmeResponseDto>());

        var controller = new FilmesController(mockService.Object);

        // Act
        await controller.GetAll();

        // Assert
        mockService.Verify(
            s => s.GetAllAsync(1, 10),
            Times.Once
        );
    }

    [Fact]
    public async Task Delete_DeveChamarServiceCorretamente()
    {
        // Arrange
        var mockService = new Mock<IFilmeService>();

        mockService
            .Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(true);

        var controller = new FilmesController(mockService.Object);

        // Act
        await controller.Delete(1);

        // Assert
        mockService.Verify(
            s => s.DeleteAsync(1),
            Times.Once
        );
    }
}

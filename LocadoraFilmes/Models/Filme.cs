using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LocadoraFilmes.Models;

public class Filme
{
    public int Id { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "O título deve conter entre 1 e 255 caracteres.")]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "O diretor deve conter entre 1 e 255 caracteres.")]
    public string Diretor { get; set; } = string.Empty;

    public string Sinopse { get; set; } = string.Empty;

    [Required]
    [Range(1888, 2100, ErrorMessage = "O ano deve ser entre 1888 e 2100.")]
    public int Ano { get; set; }

    [Required]
    public List<Genero> Generos { get; set; } = new();
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível deve ser um número inteiro não negativo.")]
    public int QuantidadeDisponivel { get; set; }

    public DateTime adicionadoEm { get; set; } = DateTime.UtcNow;
}
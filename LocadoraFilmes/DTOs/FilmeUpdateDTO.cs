using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LocadoraFilmes.DTOs;

public class FilmeUpdateDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(255, ErrorMessage = "O título deve ter no máximo 255 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O diretor é obrigatório.")]
    [StringLength(255, ErrorMessage = "O diretor deve ter no máximo 255 caracteres.")]
    public string Diretor { get; set; } = string.Empty;

    public string Sinopse { get; set; } = string.Empty;

    [Required(ErrorMessage = "O ano é obrigatório.")]
    [Range(1888, 2100, ErrorMessage = "O ano deve ser entre 1888 e 2100.")]
    public int Ano { get; set; }
    [Required(ErrorMessage = "Pelo menos um gênero é obrigatório.")]
    public List<string> Generos { get; set; } = new();
    
    [Required(ErrorMessage = "A quantidade disponível é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível deve ser um número inteiro não negativo.")]
    public int QuantidadeDisponivel { get; set; }

}
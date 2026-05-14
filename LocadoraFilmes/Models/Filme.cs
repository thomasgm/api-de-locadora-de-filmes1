using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocadoraFilmes.Models;

public class Filme
{
    public int Id { get; set; }

    [Required]
    [MaxLength(255, ErrorMessage = "O título do filme não pode exceder 255 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(255, ErrorMessage = "O diretor do filme não pode exceder 255 caracteres.")]
    public string Diretor { get; set; } = string.Empty;
    
    
    public string Sinopse { get; set; } = string.Empty;

    [Required]
    [Range(1888, 2100, ErrorMessage = "O ano deve ser entre 1888 e 2100.")]
    public int Ano { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível deve ser um número inteiro não negativo.")]
    public int QuantidadeDisponivel { get; set; } = 0;

    // Agora um filme pode ter vários gêneros (Muitos-para-Muitos)
    public ICollection<Genero> Generos { get; set; } = new List<Genero>();

    public DateTime AdicionadoEm { get; set; } = DateTime.UtcNow;
}

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LocadoraFilmes.Models;

public class Genero
{
    public int Id { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome do gênero deve conter entre 2 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;// Ex: "Drama", "Comédia"
    
    // Lista de filmes que pertencem a este gênero
    public List<Filme> Filmes { get; set; } = new();
}
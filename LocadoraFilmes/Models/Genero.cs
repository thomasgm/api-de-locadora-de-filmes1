using System.Text.Json.Serialization;

namespace LocadoraFilmes.Models;

public class Genero
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;

    public ICollection<Filme> Filmes { get; set; } = new List<Filme>();
}

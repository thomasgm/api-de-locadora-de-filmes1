namespace LocadoraFilmes.DTOs;

public class FilmeResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Diretor { get; set; } = string.Empty;
    public string Sinopse { get; set; } = string.Empty;
    public int Ano { get; set; }
    public List<string> Generos { get; set; } = new();
    public int QuantidadeDisponivel { get; set; }
    public DateTime AdicionadoEm { get; set; }
}
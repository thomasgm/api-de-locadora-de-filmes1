namespace LocadoraFilmes.Models;

public class Aluguel
{
    public int Id { get; set; }
    public int FilmeId { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataAluguel { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public bool Devolvido { get; set; }

    // Propriedades de Navegação (Relacionamentos do EF Core)
    public Filme? Filme { get; set; }
    public Cliente? Cliente { get; set; }
}

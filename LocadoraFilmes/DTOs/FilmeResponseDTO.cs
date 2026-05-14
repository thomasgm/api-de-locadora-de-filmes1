namespace LocadoraFilmes.DTOs;

public record FilmeResponseDto(
    int Id,
    string Titulo,
    string Diretor,
    int Ano,
    IEnumerable<GeneroDTO> Generos,
    int QuantidadeDisponivel,
    DateTime AdicionadoEm
);

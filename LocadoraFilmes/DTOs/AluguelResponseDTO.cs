namespace LocadoraFilmes.DTOs;

public record AluguelResponseDto (
    int Id,
    string Titulo,
    DateTime DataAluguel,
    DateTime? DataDevolucao,
    bool Devolvido
);
﻿using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Data.Dtos;

public class CreateMovieDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "O gênero é obrigatório.")]
    [StringLength(50, ErrorMessage = "O tamanho máximo do gênero é de 50 caracteres.")]
    public string Genre { get; set; }

    [StringLength(50, ErrorMessage = "O tamanho máximo do nome do(a) diretor(a) é de 50 caracteres.")]
    public string? Director { get; set; }

    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = "A duração é obrigatório.")]
    [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos.")]
    public int Duration { get; set; }
    [StringLength(200, ErrorMessage = "O tamanho máximo da descrição é de 200 caracteres.")]
    public string? Description { get; set; }
}

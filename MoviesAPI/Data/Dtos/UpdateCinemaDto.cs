using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Data.Dtos;

public class UpdateCinemaDto
{
    [Required(ErrorMessage = "O campo de nome é obrigatório.")]
    public string Name { get; set; }
}

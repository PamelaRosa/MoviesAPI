using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Data.Dtos;

public class CreateCinemaDto
{
    [Required(ErrorMessage = "O campo de nome é obrigatório.")]
    public string Name { get; set; }

    public int AddressId { get; set; }

}

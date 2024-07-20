using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Data.Dtos;

public class ReadCinemaDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

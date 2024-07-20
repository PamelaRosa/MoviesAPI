using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Models
{
    public class Movie
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O gênero é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O tamanho máximo do gênero é de 50 caracteres.")]
        public string Genre { get; set; }

        [MaxLength(50, ErrorMessage = "O tamanho máximo do nome do(a) diretor(a) é de 50 caracteres.")]
        public string? Director { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "A duração é obrigatório.")]
        [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos.")]
        public int Duration { get; set; }
        [MaxLength(200, ErrorMessage = "O tamanho máximo da descrição é de 200 caracteres.")]
        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Data.Dtos
{
    public class ReadAddressDto
    {
        [Key]
        [Required(ErrorMessage = "O campo Id é obrigatório.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Rua é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Rua deve ter no máximo 100 caracteres.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo Cidade deve ter no máximo 50 caracteres.")]
        public string City { get; set; }

        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo Estado deve ter no máximo 50 caracteres.")]
        public string State { get; set; }

        [Required(ErrorMessage = "O campo Código Postal é obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo Código Postal deve ter no máximo 10 caracteres.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "O campo País é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo País deve ter no máximo 50 caracteres.")]
        public string Country { get; set; }
    }
}

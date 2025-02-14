using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Application.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Title { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Description { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Category { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Image { get; set; }
    }

}

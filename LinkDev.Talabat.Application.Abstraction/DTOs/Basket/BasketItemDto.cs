using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Core.Application.Abstraction.DTOs.Basket
{
    public class BasketItemDto
    {
        [Required]
        public required int Id { get; set; }
        [Required]
        public required string ProductName { get; set; }
        public string? PictureUrl { get; set; }
        [Required]
        [Range(.1, double.MaxValue, ErrorMessage = "Price Must Be Greater Than Zero .")]
        public required decimal Price { get; set; }

        [Required]
        [Range(1,int.MaxValue , ErrorMessage ="Quantity Must be At Least one Item.")]
        public required int Quantity { get; set; }

         
        public required string? Brand { get; set; }
        public required string? Category { get; set; }
    }
}

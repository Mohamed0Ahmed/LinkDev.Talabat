using LinkDev.Talabat.Core.Domain.Entities.Products;
using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Dashboard.Models
{

    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]

        public required string Name { get; set; }
        [Required(ErrorMessage="Description Is Required")]
        public required string Description { get; set; }

        public string ? PictureUrl  { get; set; }
        [Required(ErrorMessage = "Price Is Required")]

        public decimal Price { get; set; }
        public ProductBrand? Brand { get; set; }
        [Required(ErrorMessage = "Brand Id Is Required")]

        public int BrandId { get; set; }

        public IFormFile ? Image { get; set; }
        public ProductCategory? Category { get; set; }
        [Required(ErrorMessage = "Category Id Is Required")]

        public int? CategoryId { get; set; }




    }
}

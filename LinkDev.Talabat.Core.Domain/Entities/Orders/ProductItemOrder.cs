using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Core.Domain.Entities.Orders
{
    public class ProductItemOrder
    {
    
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string PictureUrl { get; set; }
    }
}

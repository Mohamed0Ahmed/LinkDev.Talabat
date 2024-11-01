using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Dashboard.Models
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(255)]
        public string Name { get; set; } = null!;
    }
}

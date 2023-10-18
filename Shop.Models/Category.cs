using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Shop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Length(3, 20, ErrorMessage = "Length must be between 3-20")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Range must be between 1-100")]
        public int DisplayOrder { get; set; }

    }
}

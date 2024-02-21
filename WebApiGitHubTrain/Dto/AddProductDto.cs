using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Dto
{
    public class AddProductDto
    {

        [Required, MaxLength(100)]

        public string Name { get; set; }
        [ForeignKey(nameof(Category))]
        public int? Category_Id { get; set; }
        [ForeignKey(nameof(Brand))]
        public int? Brand_Id { get; set; }
        [Required]
        //[MaxLength(100)]
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Product_Code { get; set; }
        public IFormFile? Img { get; set; }
    }
}

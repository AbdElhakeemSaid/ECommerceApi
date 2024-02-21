using System.ComponentModel.DataAnnotations;

namespace WebApiGitHubTrain.Dto
{
    public class ProductDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Product_Code { get; set; }
        public string? Img { get; set; }
        public int? Brand_Id { get; set; }
        public string? Brand_Name { get; set; }
        public int? Category_Id { get; set; }
        public string? Category_Name { get; set; }
    }
}

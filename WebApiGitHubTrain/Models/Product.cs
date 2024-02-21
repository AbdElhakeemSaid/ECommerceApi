using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiGitHubTrain.Models
{
    public class Product
    {
        public int Id { get; set; }
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
        public string Img { get; set; }

        [JsonIgnore]
        public virtual Category? Category { get; set; }
        [JsonIgnore]

        public virtual Brand? Brand { get; set; }
        [JsonIgnore]
        public List<OrderItem>? orderItem { get; set; }
    }
}

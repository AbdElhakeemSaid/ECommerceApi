using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiGitHubTrain.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; } = 0;
        [ForeignKey(nameof(product))]
        public int Product_Id { get; set; }
        [ForeignKey(nameof(Order))]
        public int Order_Id { get; set; }

        public virtual Product? product { get; set; }
        public virtual Order? Order { get; set; }
    }
}

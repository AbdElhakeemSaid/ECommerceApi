using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiGitHubTrain.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual List<Product>? Products { get; set; }

    }
}

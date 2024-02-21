using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiGitHubTrain.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }
        [ForeignKey(nameof(Custmer))]
        public string custmer_Id { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser? Custmer { get; set; }

    }
}

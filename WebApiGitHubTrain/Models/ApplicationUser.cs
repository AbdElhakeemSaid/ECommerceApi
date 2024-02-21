using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiGitHubTrain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MinLength(3), MaxLength(20)]
        public string FirstName { get; set; }

        [Required, MinLength(3), MaxLength(20)]
        public string LastName { get; set; }
        [JsonIgnore]
        public virtual List<Order>? Order { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApiGitHubTrain.Dto
{
    public class AddToRoleModelDto
    {
        [Required]
        public string user_Id { get; set; }
        [Required]
        public string Role { get; set; }
    }
}

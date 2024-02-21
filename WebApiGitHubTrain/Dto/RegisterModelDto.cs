using System.ComponentModel.DataAnnotations;

namespace WebApiGitHubTrain.Dto
{
    public class RegisterModelDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(100)]
        public string UserName { get; set; }
        [Required, MaxLength(150)]
        public string Email { get; set; }
        [Required, MaxLength(120)]
        public string password { get; set; }
    }
}

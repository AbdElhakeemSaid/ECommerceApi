namespace WebApiGitHubTrain.Dto
{
    public class AuthModelDto
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}

namespace WebApiGitHubTrain.Dto
{
    public class OrderDetailsDto
    {
        public string? Message { get; set; }
        public string? CustomerName { get; set; }
        public double? Price { get; set; }
        public DateTime? OrderDate { get; set; }
        public Dictionary<string, int>? products { get; set; }
    }
}

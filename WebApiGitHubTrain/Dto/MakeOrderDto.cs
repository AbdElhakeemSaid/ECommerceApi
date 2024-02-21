namespace WebApiGitHubTrain.Dto
{
    public class MakeOrderDto
    {
        public string CustomerId { get; set; }
        public List<ProductInOrder> Products { get; set; }
    }
}

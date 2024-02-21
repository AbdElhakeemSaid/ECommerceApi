using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public interface IProduct
    {
        public Task<List<ProductDto>> GetAllProducts();
        public Task<Product> GetProductById(int id);
        public Task<Product> GetProductByName(string name);
        public Task<Product> GetProductByCode(string code);
        public Task<Product> Add_Product(AddProductDto model);
        public Task<Product> Update_Product(int id, Product model);
        public Task<bool> Delete_Product(int id);


    }
}

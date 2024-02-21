using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public interface IBrand
    {
        public Task<List<Brand>> GetAllBrands();
        public Task<Brand> GetBrandById(int id);
        public Task<Brand> GetBrandByName(string name);
        public Task<Brand> Add_Brand(Brand model);
        public Task<Brand> Update_Brand(int id, Brand model);
        public Task<bool> Delete_Brand(int id);
    }
}

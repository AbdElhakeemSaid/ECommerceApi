using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public interface ICategory
    {
        public Task<List<Category>> GetAllCategories();
        public Task<Category> GetCategoryById(int id);
        public Task<Category> GetCategoryByName(string name);
        public Task<Category> Add_Category(Category model);
        public Task<Category> Update_Category(int id, Category model);
        public Task<bool> Delete_Category(int id);
    }
}

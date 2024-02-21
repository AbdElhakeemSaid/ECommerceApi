using Microsoft.EntityFrameworkCore;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public class CateRepo : ICategory
    {
        private readonly DbAppContext context;

        public CateRepo(DbAppContext _context)
        {
            context = _context;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await context.Categories.FirstOrDefaultAsync(c => c.Name == name);

        }

        public async Task<Category> Add_Category(Category model)
        {
            await context.Categories.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }
        public async Task<Category> Update_Category(int id, Category model)
        {
            Category old_One = await GetCategoryById(id);
            old_One.Name = model.Name;
            await context.SaveChangesAsync();
            return old_One;
        }

        public async Task<bool> Delete_Category(int id)
        {
            Category old_One = await GetCategoryById(id);
            if (old_One is not null)
            {
                context.Categories.Remove(old_One);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

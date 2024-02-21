using Microsoft.EntityFrameworkCore;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public class BrandRepo : IBrand
    {
        private readonly DbAppContext context;

        public BrandRepo(DbAppContext _context)
        {
            context = _context;
        }

        public async Task<List<Brand>> GetAllBrands()
        {
            return await context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandById(int id)
        {
            return await context.Brands.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Brand> GetBrandByName(string name)
        {
            return await context.Brands.FirstOrDefaultAsync(b => b.Name == name);

        }

        public async Task<Brand> Add_Brand(Brand model)
        {
            await context.Brands.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }
        public async Task<Brand> Update_Brand(int id, Brand model)
        {
            Brand old_Brand = await GetBrandById(id);
            old_Brand.Name = model.Name;
            await context.SaveChangesAsync();
            return old_Brand;
        }

        public async Task<bool> Delete_Brand(int id)
        {
            Brand old_Brand = await GetBrandById(id);
            if (old_Brand != null)
            {
                context.Brands.Remove(old_Brand);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

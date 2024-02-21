using Microsoft.EntityFrameworkCore;
using WebApiGitHubTrain.Dto;
using WebApiGitHubTrain.Models;

namespace WebApiGitHubTrain.Repositories
{
    public class ProductRepo : IProduct
    {
        private readonly DbAppContext context;
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProductRepo(DbAppContext _context, IWebHostEnvironment _environment, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            environment = _environment;
            httpContextAccessor = _httpContextAccessor;
        }
        public async Task<List<ProductDto>> GetAllProducts()
        {
            List<ProductDto> products =
                await context.Products.Include(c => c.Category).Include(b => b.Brand)
                .Select(pp => new ProductDto() { Id = pp.Id, Name = pp.Name, Price = pp.Price, Quantity = pp.Quantity, Product_Code = pp.Product_Code, Img = pp.Img, Category_Name = pp.Category.Name, Category_Id = pp.Category_Id, Brand_Name = pp.Brand.Name, Brand_Id = pp.Brand_Id })
                .ToListAsync();
            if (products.Count > 0)
                return products;
            return null;
        }
        public async Task<Product> GetProductById(int id)
        {
            Product? product = await context.Products.Include(b => b.Brand).Include(c => c.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
                return null;
            return product;
        }

        public async Task<Product> GetProductByName(string name)
        {
            Product? product = await context.Products.Include(b => b.Brand).Include(c => c.Category).FirstOrDefaultAsync(p => p.Name == name);
            if (product is null)
                return null;
            return product;
        }
        public async Task<Product> GetProductByCode(string code)
        {
            Product? product = await context.Products.Include(b => b.Brand).Include(c => c.Category).FirstOrDefaultAsync(p => p.Product_Code == code);
            if (product is null)
                return null;
            return product;

        }
        public async Task<Product> Add_Product(AddProductDto model)
        {
            Product ProdModel = await FromDtoToModel(model);
            await context.Products.AddAsync(ProdModel);
            await context.SaveChangesAsync();
            return ProdModel;

        }


        public async Task<Product> Update_Product(int id, Product model)
        {
            Product old_One = await GetProductById(id);
            old_One.Name = model.Name;
            old_One.Price = model.Price;
            old_One.Quantity = model.Quantity;
            old_One.Brand_Id = model.Brand_Id;
            old_One.Category_Id = model.Category_Id;
            await context.SaveChangesAsync();
            return old_One;
        }

        public async Task<bool> Delete_Product(int id)
        {
            Product del_One = await GetProductById(id);
            if (del_One != null)
            {
                context.Products.Remove(del_One);
                await context.SaveChangesAsync();
                return true;

            }
            return false;
        }

        private async Task<Product> FromDtoToModel(AddProductDto model)
        {
            Product ProductModel = new Product()
            {
                Name = model.Name,
                Price = model.Price,
                Quantity = model.Quantity,
                Brand_Id = model.Brand_Id,
                Category_Id = model.Category_Id,
                Product_Code = model.Product_Code,
                Img = await GetImagePath(model.Img, model.Product_Code)
            };
            return ProductModel;
        }
        private async Task<string> GetImagePath(IFormFile formFile, string productCode)
        {
            string filePath = await getFilePath(productCode);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string imagePath = filePath + "\\" + productCode + ".png";
            //string url = $"{environment.EnvironmentName} - {environment.ContentRootPath}";
            var httpContext = httpContextAccessor.HttpContext;


            // Get the base URL
            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
            using (FileStream stream = File.Create(imagePath))
            {
                await formFile.CopyToAsync(stream);
            }
            string imageUrl = baseUrl + "/Upload/Product/" + productCode + "/" + productCode + ".png";
            return imageUrl;
        }
        private async Task<string> getFilePath(string productCode)
        {
            return environment.WebRootPath + "\\Upload\\Product\\" + productCode;
        }
        //    string filePath = await GetFilePath(productCode);
        //    if (!Directory.Exists(filePath))
        //    {
        //        Directory.CreateDirectory(filePath);
        //    }
        //    string imagePath = filePath + $"\\{productCode}.png";
        //    var httpContext = httpContextAccessor.HttpContext;
        //    string baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        //    if (File.Exists(imagePath))
        //    {
        //        File.Delete(imagePath);
        //    }
        //    using (FileStream stream = File.Create(imagePath))
        //    {
        //        await formFile.CopyToAsync(stream);
        //    }
        //    string ProductImage = $"{baseUrl}/Upload/Product/{productCode}/{productCode}.png";
        //    return ProductImage;
        //}

        //private async Task<string> GetFilePath(string productCode)
        //{
        //    return $"{environment.WebRootPath}\\Upload\\Product\\{productCode}";
        //}


    }
}

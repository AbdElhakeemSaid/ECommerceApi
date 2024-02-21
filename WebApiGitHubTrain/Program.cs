using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiGitHubTrain.Helper;
using WebApiGitHubTrain.Models;
using WebApiGitHubTrain.Repositories;

namespace WebApiGitHubTrain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DbAppContext>();
            builder.Services.AddDbContext<DbAppContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });
            builder.Services.AddScoped<IAuthService, AuthRepo>();
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddScoped<IProduct, ProductRepo>();
            builder.Services.AddScoped<ICategory, CateRepo>();
            builder.Services.AddScoped<IBrand, BrandRepo>();
            builder.Services.AddScoped<IOrder, OrderRepo>();
            builder.Services.AddCors(coreoption =>
            {
                coreoption.AddPolicy("myRole", corePolicybuilder =>
                {
                    corePolicybuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod();
                });
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(
                    o =>
                    {
                        o.RequireHttpsMetadata = false;
                        o.SaveToken = false;
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidIssuer = builder.Configuration["JWT:Issuer"],
                            ValidAudience = builder.Configuration["JWT:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                        };
                    }
                );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseCors("myRole");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Banking.API.Models;
using Banking.API.Repositories.Interfaces;
using Banking.API.Repositories.Repos;
using Banking.API.Repositories;

namespace Banking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                options.Audience = Configuration["Auth0:Audience"];
            });

            services.AddTransient<IAccountTypeRepo, AccountTypeRepo>();
            services.AddTransient<IAccountRepo, AccountRepo>();
            services.AddTransient<IUserRepo, UserRepo>();

            services.AddControllers();

            //Add Logging
            services.AddLogging();

            // CORS Policy definition.
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                    builder =>
                    builder.WithOrigins("http://localhost:4200", 
                                        "https://localhost:4200", 
                                        "http://localhost:5000", 
                                        "https://localhost:5000", 
                                        "http://p3ng.azurewebsites.net", 
                                        "https://p3ng.azurewebsites.net",
                                        "35.167.74.121", 
                                        "35.166.202.113", 
                                        "35.160.3.103", 
                                        "54.183.64.135", 
                                        "54.67.77.38", 
                                        "54.67.15.170", 
                                        "54.183.204.205", 
                                        "35.171.156.124", 
                                        "18.233.90.226", 
                                        "3.211.189.167")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            }
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("DefaultPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

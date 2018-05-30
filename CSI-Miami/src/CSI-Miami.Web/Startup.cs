using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CSI_Miami.Data.Models;
using CSI_Miami.Services.External;
using CSI_Miami.Data;
using System;
using AutoMapper;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Infrastructure.Providers;
using CSI_Miami.Data.Repository;
using CSI_Miami.Data.UnitOfWork;
using CSI_Miami.Services.Internal;
using CSI_Miami.Services.Internal.Contracts;
using CSI_Miami.Infrastructure.DataInitializer;

namespace CSI_Miami.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;
            this.Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.RegisterData(services);
            this.RegisterAuthentication(services);
            this.RegisterServices(services);
            this.RegisterInfrastructure(services);

            return services.BuildServiceProvider();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IEmailSender, EmailSender>();

        }

        private void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMemoryCache();

            services.AddScoped<IMappingProvider, MappingProvider>();
            services.AddScoped<IUserManagerProvider, UserManagerProvider>();
            services.AddScoped<IDataInitializer, DataInitializer>();


            services.AddAutoMapper(options =>
            {
                options.AddProfile<MappingSettings>();
            });

        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Authorize");

            if (this.Environment.IsDevelopment())
            {
                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
                    options.Lockout.MaxFailedAccessAttempts = 999;
                });
            }
        }

        private void RegisterData(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
            services.AddScoped<IDataSaver, DataSaver>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IDataInitializer dataInitializer, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            context.Database.Migrate();
            dataInitializer.Initialize();

        }
    }
}

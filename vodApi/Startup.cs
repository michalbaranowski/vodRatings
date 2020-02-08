using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using vod.Core;
using vod.Core.Boundary.Interfaces;
using vod.Domain.Services;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Serialize;
using vod.Repository;
using vod.Repository.Boundary;
using vod.SignalR.Hub.Hub;

namespace vodApi
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")).Options;
            services.AddSingleton(dbContextOptions);
            services.AddTransient<IAppDbContext, AppDbContext>();

            services.AddDbContext<AuthDbContext>(option => option.UseSqlServer(Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(
                option =>
                {
                    option.Password.RequireDigit = true;
                    option.Password.RequiredLength = 6;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = true;
                    option.Password.RequireLowercase = true;
                }
            ).AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Site"],
                    ValidIssuer = Configuration["Jwt:Site"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                };
            });

            services.AddHttpClient();
            services.AddSingleton<UpdateNotificationHub>();
            services.AddSingleton<IBackgroundWorkerStateManager, BackgroundWorkerStateManager>();
            services.AddSingleton<IBackgroundWorker, BackgroundWorker>();
            services.AddTransient<IVodRepositoryBackground, VodRepositoryBackground>();
            services.AddTransient<IVodRepository, VodRepository>();
            services.AddTransient<IStoredDataManager, StoredDataManager>();
            services.AddTransient<IUrlGetter, UrlGetter>();
            services.AddTransient<INcPlusService, NcPlusService>();
            services.AddTransient<IFilmwebService, FilmwebService>();
            services.AddTransient<IHtmlSourceGetter, HtmlSourceGetter>();
            services.AddTransient<IHtmlSourceSerializer, HtmlSourceSerializer>();
            services.AddTransient<IFilmwebResultsProvider, FilmwebResultsProvider>();
            services.AddTransient<ICoreLogic, CoreLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<UpdateNotificationHub>("/updateNotification");
            });

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

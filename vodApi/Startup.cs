using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            Configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables()
            .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddSignalR();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var dbContextOptions = _hostingEnvironment.IsProduction() 
                ? new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(Configuration.GetConnectionString("SQLCONNSTR_DefaultConnection")).Options 
                : throw new Exception("give me connstr");

            services.AddSingleton(dbContextOptions);
            services.AddTransient<IAppDbContext, AppDbContext>();

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
            app.UseMvc();
        }
    }
}

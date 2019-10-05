using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using vod.Core;
using vod.Core.Boundary;
using vod.Core.Boundary.Interfaces;
using vod.Domain.Services;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;
using vod.Repository;
using vod.Repository.Boundary;

namespace vodApi
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
            services.AddAutoMapper(typeof(Startup));

            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(Configuration.GetConnectionString("VodConnection")).Options;
            services.AddSingleton(dbContextOptions);
            services.AddTransient<IAppDbContext, AppDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpClient();
            services.AddTransient<IBackgroundWorker, BackgroundWorker>();
            services.AddTransient<IVodRepositoryBackground, VodRepositoryBackground>();
            services.AddTransient<IVodRepository, VodRepository>();
            services.AddTransient<IStoredDataManager, StoredDataManager>();
            services.AddTransient<INcPlusService, NcPlusService>();
            services.AddTransient<IFilmwebService, FilmwebService>();
            services.AddTransient<IHtmlSourceGetter, HtmlSourceGetter>();
            services.AddTransient<IHtmlSourceDeserializer, HtmlSourceDeserializer>();
            services.AddTransient<ICoreLogic, CoreLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using vod.Core;
using vod.Core.Boundary;
using vod.Domain.Services;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpClient();
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

            app.UseMvc();
        }
    }
}

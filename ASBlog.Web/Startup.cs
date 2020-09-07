using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ASBlog.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using ASBlog.Web.Settings;
using Blazored.LocalStorage;
using ASBlog.Web.Services;
using System.Net.Http;

namespace ASBlog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();


            // App URL
            var appSettingSection = Configuration.GetSection("App");
            services.Configure<AppSettings>(appSettingSection);


            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            services.AddBlazoredLocalStorage();
            services.AddHttpClient<IUserService, UserService>();
            services.AddHttpClient<IBlogService, BlogService>();


            services.AddSingleton<HttpClient>();

            services.AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapFallbackToAreaPage("/Admin/{*clientroutes:nonfile}", "/_HostAdmin", "Admin");
            });
        }
    }
}

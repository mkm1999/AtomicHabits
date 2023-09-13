using Application.GetUsers;
using Application.Interfaces.Context;
using Application.ReportsService.Command.AddReportService;
using Application.ReportsService.Query.GetHomeDataService;
using Application.ReportsService.Query.GetReportService;
using Application.UserService.Command.Deleate;
using Application.UserService.Command.Edit;
using Application.UserService.Command.EditPurposeService;
using Application.UserService.Command.Register;
using Application.UserService.Query.GetRoles;
using Application.UserService.Query.ILoginService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ui
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
            string Connection = Configuration["ConnectionString:Connection"];

            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Authentication/SignIn";
                    options.LoginPath = "/Authentication/SignIn";
                });
            services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(Connection));


            //DI
            services.AddScoped<IDataBaseContext , DataBaseContext>();
            services.AddScoped<IGetUsers , GetUsers>();
            services.AddScoped<IGetRolesService, GetRolesService>();
            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IDeleateUser, DeleateUser>();
            services.AddScoped<IEditUserService,EditUserService>();
            services.AddScoped<ILogin, Login>();
            services.AddScoped<IAddReport, AddReport>();
            services.AddScoped<IEditPurpose,EditPurpose>();
            services.AddScoped<IGetReport,GetReport>();
            services.AddScoped<IGetDaysReport,GetDaysReport>();
            services.AddScoped<IGetHomeData,GetHomeData>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                
            });
        }
    }
}

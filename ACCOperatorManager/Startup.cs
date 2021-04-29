using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccOperatorManager.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Logging;

namespace AccOperatorManager
{
    public class Startup
    {
        const string connStrPattern = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=server_ip)(PORT=1521)))(CONNECT_DATA=(SID = server_sid)));User Id=server_user ;Password=acc";

        const string connStrB404 = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.213.7.1)(PORT=1521)))(CONNECT_DATA=(SID = xe)));User Id=acc ;Password=acc";
        const string connStrB403 = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.213.11.1)(PORT=1521)))(CONNECT_DATA=(SID = xe)));User Id=acc ;Password=acc";


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContextPool<AccOperatorManagerDbContext>(options =>
            //{
            //    options.UseOracle(connStrB404);
            //});

            //services.AddDbContext<AccDbContext>(options => 
            //    options.UseOracle(connStrB403, options => 
            //        options.UseOracleSQLCompatibility("11")));

            services.AddDbContextFactory<AccDbContext>(options =>
                options.UseOracle(connStrPattern, options =>
                    options.UseOracleSQLCompatibility("11")));

            services.AddScoped<IAccOperatorData, OracleAccOperatorData>();
            services.AddRazorPages();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AccOperatorValidator>());
            services.Configure<List<Line>>(Configuration.GetSection("AccLines"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/AccOperatorManagerLog-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

using Demo.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;

namespace Demo
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
            services.AddRazorPages();
            
            services.AddScoped<IInterceptor, SqlLoggingInterceptor>();
            
            // configure NHibernate
            services.AddSingleton(svc =>
            {
                return Fluently
                    .Configure()
                    .Database(
                        SQLiteConfiguration
                        .Standard
                        .UsingFile(Database.DatabaseFilename)
                    )
                    .Mappings(m =>
                        m
                        .FluentMappings
                        .AddFromAssemblyOf<Program>()
                    )
                    .ExposeConfiguration(Database.BuildSchema)
                    .BuildSessionFactory();
            });

            services.AddScoped<ISession>(svc =>
            {
                var interceptor = svc.GetRequiredService<IInterceptor>();
                var factory = svc.GetRequiredService<ISessionFactory>();
                return factory
                    .OpenSession()
                    .SessionWithOptions()
                    .Interceptor(interceptor)
                    .OpenSession();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISession session)
        {
            Database.Initialize(session);
            
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

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}
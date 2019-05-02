namespace SSMB.Web
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SQL;
    using System;
    using Hangfire.SqlServer;
    using Modules;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            var options = new BackgroundJobServerOptions
            {
                WorkerCount = 2,
                Queues = new[] { "gather_checkable", "update_mc", "check_available" }
            };

            app.UseHangfireServer(options);
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContext<SsmbDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("SSMBDatabase")));
            services.AddHangfire(x => x.UseSqlServerStorage(this.Configuration.GetConnectionString("SSMBDatabase")));

            JobStorage.Current = new SqlServerStorage(this.Configuration.GetConnectionString("SSMBDatabase"),
                new SqlServerStorageOptions());

            var builder = new ContainerBuilder();
            builder.RegisterModule<SSMB.DataCollection.Modules.DataCollectionModule>();
            builder.Populate(services);
            var container = builder.Build();

            GlobalConfiguration.Configuration.UseAutofacActivator(container);
            return new AutofacServiceProvider(container);
        }
    }
}

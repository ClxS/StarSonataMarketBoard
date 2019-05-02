namespace SSMB.Blazor
{
    using System;
    using System.Net.Http.Headers;
    using System.Reflection;
    using Application.Infrastructure;
    using Application.Interfaces;
    using Application.Items.Queries.GetHotItems;
    using Domain;
    using Filters;
    using FluentValidation.AspNetCore;
    using Hangfire;
    using Hangfire.SqlServer;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Server.API.V1;
    using Services;
    using Shared;
    using Shared.ItemsResults;
    using Shared.ItemsSearch;
    using Shared.RecentItems;
    using SQL;
    using ViewServices;
    using ViewServices.Defaults;
    using GetRecentItemsQuery = Application.Items.Queries.GetRecentItems.GetRecentItemsQuery;

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                endpoints.MapControllerRoute("default", "api/v1/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("ssmb", c =>
            {
                c.BaseAddress = new Uri("http://localhost:44327");
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddDbContext<ISsmbDbContext, SsmbDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("SSMBDatabase")));
            this.RegisterHangfireTypes(services);
            this.RegisterMediatRType(services);
            services.AddHttpClient();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                    .AddApplicationPart(typeof(ItemsController).Assembly)
                    .AddControllersAsServices()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetHotItemsQueryValidator>());
            this.RegisterBlazorTypes(services);
        }

        private void RegisterBlazorTypes(IServiceCollection services)
        {
            services.AddTransient<IItemSearchViewModel, ItemsSearchViewModel>();
            services.AddTransient<IItemsResultsViewModel, ItemsResultsViewModel>();
            services.AddTransient<IMainLayoutViewModel, MainLayoutViewModel>();
            services.AddTransient<IRecentItemsViewModel, RecentItemsViewModel>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IItemsService, ItemsServiceServerBased>();
        }

        private void RegisterHangfireTypes(IServiceCollection services)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(this.Configuration.GetConnectionString("SSMBDatabase")));
            JobStorage.Current = new SqlServerStorage(this.Configuration.GetConnectionString("SSMBDatabase"),
                new SqlServerStorageOptions());
        }

        private void RegisterMediatRType(IServiceCollection services)
        {
            services.AddMediatR(typeof(GetRecentItemsQuery).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
        }
    }
}

namespace SSMB.Blazor
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Claims;
    using System.Text.Json;
    using Application.Infrastructure;
    using Application.Interfaces;
    using Application.Items.Queries.GetRecentItems;
    using DataCollection;
    using Filters;
    using Hangfire;
    using Hangfire.SQLite;
    using MediatR;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OAuth;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Migrations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using Pages.Appraise;
    using Pages.CreateAlert;
    using Pages.Index.HottestItems;
    using Pages.Index.ProfitableItems;
    using Pages.Index.RecentItems;
    using Pages.Item;
    using Pages.Item.LatestHistory;
    using Pages.ScrapShop;
    using Pages.UnderCut;
    using Server.API.V1;
    using Services;
    using Shared;
    using Shared.AccountPanel;
    using Shared.ItemsResults;
    using Shared.ItemsSearch;
    using SQL;
    using ViewServices;
    using ViewServices.Defaults;

    public class Startup
    {
        private const string DbConnectionKey = "SSMBDatabase";

        private readonly SqliteConnection sqlliteConnection;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.sqlliteConnection = new SqliteConnection(this.Configuration.GetConnectionString(DbConnectionKey));
            this.sqlliteConnection.Open(); // TODO[CJ] Bad!
            OpenBrowser("http://localhost:5050/");
        }

        public IConfiguration Configuration { get; }

        private static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();

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

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                ((SsmbDbContext)scope.ServiceProvider.GetService<ISsmbDbContext>()).Database.Migrate();
            }
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
                options.UseSqlite(this.Configuration.GetConnectionString(DbConnectionKey)), ServiceLifetime.Transient);
            this.RegisterHangfireTypes(services);
            this.RegisterMediatRType(services);
            services.AddHttpClient();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                    .AddApplicationPart(typeof(ItemsController).Assembly)
                    .AddControllersAsServices();
            services.AddAuthentication(
                        (options) =>
                        {
                            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = "Discord";
                        })
                        .AddCookie();
                        /*.AddOAuth("Discord", options =>
                        {
                            options.ClientId = this.Configuration["Discord:ClientId"];
                            options.ClientSecret = this.Configuration["Discord:ClientSecret"];
                            options.CallbackPath = new PathString("/discord-auth");
                            options.Scope.Add("identify");
                            options.AuthorizationEndpoint = "https://discordapp.com/api/oauth2/authorize";
                            options.TokenEndpoint = "https://discordapp.com/api/oauth2/token";
                            options.UserInformationEndpoint = "https://discordapp.com/api/users/@me";

                            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
                            options.ClaimActions.MapJsonKey("urn:discord:avatar", "avatar");

                            options.Events = new OAuthEvents
                            {
                                OnCreatingTicket = async context =>
                                {
                                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                                    var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                                    response.EnsureSuccessStatusCode();

                                    var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                                    context.RunClaimActions(user.RootElement);
                                }
                            };
                        });*/
            services.AddSingleton<ILoginCredentials>(_ =>
            {
                var credentialsSection = this.Configuration.GetSection("Credentials");
                if (credentialsSection == null)
                {
                    throw new Exception("Credentials section is missing from appsettings.json");
                }

                var username = credentialsSection["Username"];
                var password = credentialsSection["Password"];
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new Exception("Credentials section is missing a username or password");
                }

                return new ConfigCredentials(username, password);
            });

            this.RegisterBlazorTypes(services);
        }

        private void RegisterBlazorTypes(IServiceCollection services)
        {
            services.AddTransient<IScrapShopViewModel, ScrapShopViewModel>();
            services.AddTransient<IItemSearchViewModel, ItemsSearchViewModel>();
            services.AddTransient<IItemsResultsViewModel, ItemsResultsViewModel>();
            services.AddTransient<IMainLayoutViewModel, MainLayoutViewModel>();
            services.AddTransient<IRecentItemsViewModel, RecentItemsViewModel>();
            services.AddTransient<IHottestItemsViewModel, HottestItemsViewModel>();
            services.AddTransient<IItemViewModel, ItemViewModel>();
            services.AddTransient<ILatestHistoryViewModel, LatestHistoryViewModel>();
            services.AddTransient<IAccountPanelViewModel, AccountPanelViewModel>();
            services.AddTransient<ICreateAlertViewModel, CreateAlertViewModel>();
            services.AddTransient<IProfitableItemsViewModel, ProfitableItemsViewModel>();
            services.AddTransient<IAppraiseViewModel, AppraiseViewModel>();
            services.AddTransient<IUnderCutViewModel, UnderCutViewModel>(); 
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IItemsService, ItemsServiceServerBased>();
            services.AddScoped<IAlertsService, AlertsServiceServerBased>();
        }

        private bool IsConnectionString(string nameOrConnectionString)
        {
            return nameOrConnectionString.ToLower().Contains("data source");
        }

        private void RegisterHangfireTypes(IServiceCollection services)
        {
            services.AddHangfire(x => { x.UseSQLiteStorage(this.Configuration.GetConnectionString("SSMBDatabase")); });
            JobStorage.Current = new SQLiteStorage(this.Configuration.GetConnectionString("SSMBDatabase"), new SQLiteStorageOptions());
        }

        private void RegisterMediatRType(IServiceCollection services)
        {
            services.AddMediatR(typeof(GetRecentItemsQuery).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));
        }

        private class ConfigCredentials : ILoginCredentials
        {
            public ConfigCredentials(string username, string password)
            {
                this.Username = username;
                this.Password = password;
            }

            public string Username { get; }

            public string Password { get; }
        }
    }
}

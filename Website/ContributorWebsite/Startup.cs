using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublicWebsite.APIClient;

namespace ContributorWebsite
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
            services.AddAuthentication(AzureADDefaults.AuthenticationScheme) //sharedOptions =>
            //{
            //    //sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})
            .AddAzureAD(options => Configuration.Bind("AzureAd", options));
            //.AddCookie();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            //{
            //    options.Authority = options.Authority + "/v2.0/";
            //    options.TokenValidationParameters.ValidateIssuer = false;
            //});

            //services.AddIdentity<ApplicationUser, IdentityRole>();

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            {
                options.Authority = options.Authority + "/v2.0/";         // Azure AD v2.0
                options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            });


            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // DI Config
            services.AddSingleton<APIClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }





        // TODO This is all pulled from the old API that is now removed. Need to integrate this into the migrated-to API (this project).
        //////public Startup(IConfiguration configuration)
        //////{
        //////    Configuration = configuration;
        //////}

        //////public IConfiguration Configuration { get; }

        //////// This method gets called by the runtime. Use this method to add services to the container.
        //////public void ConfigureServices(IServiceCollection services)
        //////{
        //////    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        //////    var dbConnString = Configuration["DatabaseConnectionString"];

        //////    services.AddDbContext<DbPackageRepository>(options => options.UseSqlServer(dbConnString));
        //////    services.AddScoped<IPackageRepository, DbPackageRepository>();
        //////}
    }
}

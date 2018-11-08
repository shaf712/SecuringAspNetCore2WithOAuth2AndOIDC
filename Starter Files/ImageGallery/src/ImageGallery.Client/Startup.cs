using ImageGallery.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ImageGallery.Client
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
 
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // register an IHttpContextAccessor so we can access the current
            // HttpContext in services by injecting it
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // register an IImageGalleryHttpClient
            services.AddScoped<IImageGalleryHttpClient, ImageGalleryHttpClient>();

            //we need a way to store the user's identity 
            
            //ADD THE AUTHENTICATION AND OIDC WORKFLOW

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies")  //pass in the scheme name to AddCookie method -> allows the application to use cookie-based authentication for our default scheme 
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = "https://localhost:44387";
                options.ClientId = "imagegalleryclient";
                options.ResponseType = "code id_token";
				//options.SignedOutCallbackPath = new PathString("..."); 
                options.Scope.Add("openid");
                options.Scope.Add("profile");
				options.Scope.Add("address");
				options.Scope.Add("email");
				options.Scope.Add("roles");
				options.SaveTokens = true;
                options.ClientSecret = "secret";
				options.ClaimActions.MapUniqueJsonKey("role", "role"); 
				//initialize UserInfo endpoint to get claims from the scopes that are passed in
				options.GetClaimsFromUserInfoEndpoint = true; 

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            app.UseAuthentication(); //use the authentication that we created above 

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Gallery}/{action=Index}/{id?}");
            });
        }
    }
}

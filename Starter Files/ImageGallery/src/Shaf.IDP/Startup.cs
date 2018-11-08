using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Shaf.IDP
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(); 
            //register the Identity Server services on ASP.Net Core's built-in dependency container 
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddTestUsers(Config.GetUsers())
                //IN-MEMORY methods for now - this will be different 
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients());
                //sets up a credential to sign the tokens with - should be changed to a REAL certificate in Production
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //add Identity Server to the request pipeline
            app.UseIdentityServer();
            app.UseStaticFiles();
            //UseStaticFiles() method is called because it's able to mark files in web root as servable. 
            //For example: href="~/favicon.ico" The tilde "~" sign points to webroot (wwwroot) so the real URL is: wwwroot/favicon.ico
            //Without this method, the image path would not be valid and nothing would show. 
            app.UseMvcWithDefaultRoute(); 
        }
    }
}

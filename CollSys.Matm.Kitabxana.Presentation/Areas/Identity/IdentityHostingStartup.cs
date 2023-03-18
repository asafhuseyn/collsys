using System;
using CollSys.Matm.Kitabxana.Presentation.Areas.Identity.Data;
using CollSys.Matm.Kitabxana.Presentation.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CollSys.Matm.Kitabxana.Presentation.Areas.Identity.IdentityHostingStartup))]
namespace CollSys.Matm.Kitabxana.Presentation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
                        {
                            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                        });
            app.UseAuthentication();
        }
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("UserContextConnection")));

                services.AddDefaultIdentity<UserModel>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<UserContext>();
            });
        }
    }
}
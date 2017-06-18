using Kachuwa.Log;
using Kachuwa.Web.Module;
using Kachuwa.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kachuwa.Web
{
    public static class KachuwaWebExtensionsHelper
    {
        public static IServiceCollection RegisterKachuwaWebServices(this IServiceCollection services)
        {
            
            services.AddSingleton<IScriptRunner, SQLScriptRunner>();
            services.AddSingleton<IModuleService, ModuleService>();
            services.AddSingleton<IModuleManager, ModuleManager>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<ISmsSender, SmsSender>();
            //var ctxaccessor = services.BuildServiceProvider().GetService<IHttpContextAccessor>();
            //var ctx = new ContextResolver(ctxaccessor);
            //services.AddSingleton(ctx);
            var logger = services.BuildServiceProvider().GetService<ILogger>();
            var modules = new ModuleRegistrar(services, logger);
            return services;
        }
        public static IApplicationBuilder UseKachuwaWeb(this IApplicationBuilder app)
        {
            app.UseMiddleware<ModuleResourceMiddleware>();
            
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions()
            {
                Authority = "http://kachuwaframework.com",
                ApiName = "",
                ApiSecret = "",
                RequireHttpsMetadata = false
                
            });
            return app;
        }
    }
}
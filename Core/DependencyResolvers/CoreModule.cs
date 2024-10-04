using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DependencyResolvers
{

    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICacheManager,MemoryCacheManager>();
            services.AddSingleton<Stopwatch>();


            //services.AddSwaggerGen(c =>
            //{
            //    //c.SwaggerDoc(SwaggerMessages.Version, new OpenApiInfo
            //    //{
            //    //    Version = SwaggerMessages.Version,
            //    //    Title = SwaggerMessages.Title,
            //    //});

            //    //c.OperationFilter<AddAuthHeaderOperationFilter>();
            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Description = "`Token only!!!` - without `Bearer_` prefix",
            //        Type = SecuritySchemeType.ApiKey,
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Scheme = "bearer"
            //    });

            //});

        }
    }
}

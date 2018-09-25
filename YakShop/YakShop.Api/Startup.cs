using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using YakShop.Api.Infrastructure;

namespace YakShop.Api
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
            services.AddMvc(options =>
            {
                options.FormatterMappings.SetMediaTypeMappingForFormat
                    ("xml", MediaTypeHeaderValue.Parse("application/xml").ToString());
                options.FormatterMappings.SetMediaTypeMappingForFormat
                    ("js", MediaTypeHeaderValue.Parse("application/json").ToString());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddXmlSerializerFormatters()
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            });

            services.AddDbContext<YakShopContext>(context => 
            {
                context.UseInMemoryDatabase("YakShopDB");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("yak-shop", new Info { Title = "Yak Shop API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/yak-shop/swagger.json", "Yak Shop API");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}

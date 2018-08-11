using Dashboard.Context;
using Dashboard.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Dashboard
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvcCore();
            mvcCoreBuilder
            .AddFormatterMappings()
            .AddJsonFormatters()
            .AddCors();

            services.AddDbContext<DashboardDB>();
            services.AddCors();
            services.AddMvc();

            Services.OrderChangedSBR.ReceiveAsync();
            Services.UserWithLessOffer.ReceiveAsync();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info { Title = "Dashboard", Version = "v2" });
            });
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "GeekBurger Dashboard API");
                c.RoutePrefix = string.Empty;
            });
            app.UseMvc();
        }
    }
}

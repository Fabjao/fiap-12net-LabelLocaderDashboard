﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Dashboard.Services;
using Microsoft.EntityFrameworkCore;
using Dashboard.Context;

namespace Dashboard
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //var mvcCoreBuilder = services.AddMvcCore();
            //mvcCoreBuilder
            //.AddFormatterMappings()
            //.AddJsonFormatters()
            //.AddCors();

            //services.AddDbContext<Contexto>(opt => opt.UseInMemoryDatabase("TestDatabase"));

            services.AddMvc();
            services.AddCors(); 
            

            ServiceBusReceive serviceBus = new ServiceBusReceive();

            serviceBus.ReceiveAsync("8048e9ec-80fe-4bad-bc2a-e4f4a75c834e");
            serviceBus.ReceiveAsync("8d618778-85d7-411e-878b-846a8eef30c0");

            serviceBus.ReceiveAsyncUserUsersWithLessOffer("8048e9ec-80fe-4bad-bc2a-e4f4a75c834e");
            serviceBus.ReceiveAsyncUserUsersWithLessOffer("8d618778-85d7-411e-878b-846a8eef30c0");


            //Services.ServiceBusReceive.ReceiveAsync("8048e9ec-80fe-4bad-bc2a-e4f4a75c834e");

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "Dashboard", Version = "v1" });
            //    //c.IncludeXmlComments(@"bin\x64\Debug\netcoreapp2.0\Dashboard.xml");
            //});
        }

    
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            //var context = app.ApplicationServices.GetService<Contexto>();
            //AddTestData(context);


            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = string.Empty;
            //});
            app.UseMvc();
        }

        private static void AddTestData(Contexto context)
        {
            var valor = 10.00;
            var testUser1 = new Context.OrderChanged
            {
                OrderId = 1,
                State = "Paid",
                StoredId = "110901091019901",
                Value = valor
            };

            context.OrderChanged.Add(testUser1);

            context.SaveChanges();
        }
    }
}

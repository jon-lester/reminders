using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JL.Reminders.Api.Models;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;
using JL.Reminders.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace JL.Reminders.Api
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
            services.AddMvc();

	        services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();
	        services.AddSingleton<IRemindersRepository, RemindersRepository>();
	        services.AddSwaggerGen(c =>
	        {
		        c.SwaggerDoc("v1", new Info()
		        {
			        Title = "Reminders API",
			        Description = "WebAPI for the Reminders app"
		        });
		        c.IncludeXmlComments(string.Format(@"{0}\JL.Reminders.Api.XML", System.AppDomain.CurrentDomain.BaseDirectory));
			});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
	            app.UseSwagger(c =>
	            {
	            });
	            app.UseSwaggerUI(c =>
	            {
		            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reminders v1");
				});
			}

	        app.UseMvc();

			ConfigureAutoMapper();
        }

	    private void ConfigureAutoMapper()
	    {
		    Mapper.Initialize(config =>
		    {
			    config.CreateMap<ReminderDetailsModel, Reminder>();
		    });
		}
    }
}

using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using JL.Reminders.Api.Automapper;
using JL.Reminders.Api.Models;
using JL.Reminders.Core.Model;
using JL.Reminders.Services.Automapper;

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
			services.AddCors(corsOptions =>
			{
				corsOptions.AddPolicy("RemindersUI", builder =>
				{
					builder
						.WithOrigins("http://localhost:3000")
						.WithHeaders("Content-Type", "Authorization")
						.WithMethods("GET", "POST", "PUT", "DELETE", "PATCH");
				});
			});

			services.AddAuthentication(authOptions =>
	        {
		        authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		        authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	        })
		        .AddJwtBearer(bearerOptions =>
		        {
			        bearerOptions.Authority = Configuration.GetSection("Authorisation")["Authority"];
			        bearerOptions.Audience = Configuration.GetSection("Authorisation")["Audience"];
					bearerOptions.TokenValidationParameters = new TokenValidationParameters
			        {
						ValidateIssuerSigningKey = true,
				        ValidateAudience = true,
				        ValidateIssuer = true,
				        ValidateLifetime = true
			        };
		        });

			services.AddMvc(mvcOptions =>
		        {
		        })
		        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
		        .AddFluentValidation(vc => vc.RegisterValidatorsFromAssemblyContaining<Startup>());

			services.AddRemindersApp();

	        services.AddSwaggerGen(c =>
	        {
		        c.SwaggerDoc("v1", new Info()
		        {
			        Title = "Reminders API",
			        Description = "WebAPI for the Reminders app"
		        });
		        // turn this on later when everything is xml-doc'd to avoid annoying missing-xmldoc whinging
		        // c.IncludeXmlComments(string.Format(@"{0}\JL.Reminders.Api.XML", System.AppDomain.CurrentDomain.BaseDirectory));
	        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

	            app.UseSwagger();

	            app.UseSwaggerUI(c =>
	            {
		            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reminders v1");
				});
			}

	        app.UseAuthentication();

	        app.UseMvc();

			ConfigureAutoMapper();
        }

	    private void ConfigureAutoMapper()
	    {
		    Mapper.Initialize(config =>
		    {
			    config.AddProfiles(
				    typeof(ApiAutomapperProfile),
				    typeof(ServicesAutomapperProfile));
		    });
		}
    }
}

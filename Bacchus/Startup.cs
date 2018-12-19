﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bacchus.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bacchus
{
	public class Startup
	{
		public Startup( IConfiguration configuration ) =>
			Configuration = configuration;

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices( IServiceCollection services )
		{
			services.AddDbContext<ApplicationDbContext>( options =>
				 options.UseSqlServer(
					 Configuration["Data:Bacchus:ConnectionString"] ) );
			services.AddHttpClient<IUptimeAuctionApiClient, UptimeAuctionApiClient>();
			services.AddTransient<IProductRepository, EFProductRepository>();
			services.AddMvc();
			services.AddMemoryCache();
			services.AddSession();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app, IHostingEnvironment env )
		{
			//if( env.IsDevelopment() )
			//{
				app.UseDeveloperExceptionPage();
			//}
			//else
			//{
			//	//TODO User friendly ErrorPage
			//}
			app.UseStatusCodePages();
			app.UseStaticFiles();
			app.UseSession();
			app.UseMvc( routes => {
				routes.MapRoute(
					name: null,
					template: "{category}/Page{productPage:int}",
					defaults: new { controller = "Product", action = "List" }
				);

				routes.MapRoute(
					name: null,
					template: "Page{productPage:int}",
					defaults: new { controller = "Product", action = "List", productPage = 1 }
				);

				routes.MapRoute(
					name: null,
					template: "{category}",
					defaults: new { controller = "Product", action = "List", productPage = 1 }
				);

				routes.MapRoute(
					name: null,
					template: "",
					defaults: new { controller = "Product", action = "List", productPage = 1 } );

				routes.MapRoute( name: null, template: "{controller}/{action}/{id?}" );
			} );

			// auto create the database
			using( var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope() )
			{
				ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				if( !context.Database.EnsureCreated() )
				{
					context.Database.Migrate();
				}
			}

		}
	}
}

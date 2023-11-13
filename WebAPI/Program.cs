
using LiBaby.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Logging.ClearProviders();
			builder.Logging.AddSerilog(
				new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger()
			);
			builder.Services.AddAutoMapper(typeof(LiBaby.ViewModels.DataModelMapping));
			builder.Services.AddDbContext<KpzDbContext>(options =>
				options.UseMySql(builder.Configuration.GetConnectionString("sqlConnection"),
					ServerVersion.Parse("8.0.32-mysql"))
			);


			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "LiBaby API",
					Description = "An ASP.NET Core Web API for managing books in a library.",
					Contact = new OpenApiContact
					{
						Name = "Telegram",
						Url = new Uri("https://t.me/maksymko")
					}
				});
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
			});


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();
			app.UseStaticFiles();

			app.MapControllers();

			app.Run();
		}
	}
}

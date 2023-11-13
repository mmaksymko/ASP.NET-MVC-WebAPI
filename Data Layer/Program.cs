using LiBaby.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<KpzDbContext>(options =>
				options.UseMySql(builder.Configuration.GetConnectionString("sqlConnection"),
					ServerVersion.Parse("8.0.32-mysql"))
			);
			var app = builder.Build();
		}
	}
}

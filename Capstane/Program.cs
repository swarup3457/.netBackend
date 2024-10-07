using CapstoneDAL;
using CapstoneDAL.Models;
using Capstane.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TicketManagement.Services;
using TicketManagement.Repositories;
using Capstone.Services;

namespace Capstane
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Register the DbContext with the correct migrations assembly
			builder.Services.AddDbContext<CapstoneDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly("CapstoneDAL"))); // Ensure this is correct

            // Register the UserService and UserLoginService
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserLoginService>();
            builder.Services.AddScoped<AgentRepository>();
            builder.Services.AddScoped<DisplayAgentRepository>();
            builder.Services.AddScoped<DisplayAgentService>();
            builder.Services.AddScoped<ITicketService,TicketService>(); // Register ITicketService with its implementation
            builder.Services.AddScoped<TicketService>();
            builder.Services.AddScoped<AdminService>();


            builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins", builder =>
				{
					builder.AllowAnyOrigin()       // Allow all origins
						   .AllowAnyMethod()       // Allow all HTTP methods
						   .AllowAnyHeader();      // Allow all headers
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication(); // Ensure this is added
			app.UseCors("AllowAllOrigins");

			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}

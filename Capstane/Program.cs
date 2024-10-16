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

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<CapstoneDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly("CapstoneDAL")));

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserLoginService>();
            builder.Services.AddScoped<AgentRepository>();
            builder.Services.AddScoped<DisplayAgentRepository>();
            builder.Services.AddScoped<DisplayAgentService>();
            builder.Services.AddScoped<ITicketService,TicketService>(); 
            builder.Services.AddScoped<TicketService>();
            builder.Services.AddScoped<AdminService>();

            builder.Services.AddTransient< TicketService>(); 

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddTransient<PdfGenerator>();


			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins", builder =>
				{
					builder.AllowAnyOrigin()       
						   .AllowAnyMethod()      
						   .AllowAnyHeader();     
				});
			});


			builder.Services.AddControllers();
			builder.Services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddConsole();
				loggingBuilder.AddDebug();  
			});

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			app.UseCors("AllowAllOrigins");

			app.UseHttpsRedirection();
			app.UseAuthentication();

			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}


using BookingBirthday.Application.IServices;
using BookingBirthday.Application.Services;
using BookingBirthday.Data.EF;
using BookingBirthday.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace BookingBirthday.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.

            builder.Services.AddControllers();

            // Add services to the container.
            builder.Services.AddDbContext<BookingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookingBirthdayDb")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            // Life cycle DI: AddSingleton(), AddTransient(), AddScoped()
            builder.Services.AddScoped<IPackageService, BookingBirthday.Application.Services.PackageService>();
            builder.Services.AddScoped<IUserService, UserService>();


            var app = builder.Build();



            // //Initialize the database
            //var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            //using (var scope = scopeFactory.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<DbContextOptions>();
            //    db.Database.EnsureCreated();
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("corspolicy");


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

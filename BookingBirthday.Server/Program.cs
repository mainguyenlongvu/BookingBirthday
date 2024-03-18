using BookingBirthday.Server.Services;
using BookingBirthday.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace BookingBirthday.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BookingDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("BookingBirthdayDb"));
            });

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BookingDbContext>();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IVnPayService, VnPayService>();
            builder.Services.AddAuthentication()
            .AddGoogle(googleOptions =>
            {
                // Đọc thông tin Authentication:Google từ appsettings.json
                IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

                // Thiết lập ClientID và ClientSecret để truy cập API google
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
                googleOptions.CallbackPath = "/signin-google";

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

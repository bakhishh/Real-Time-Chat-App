using Microsoft.EntityFrameworkCore;
using ChatApp.Data;
using ChatApp.Data.Interfaces;
using ChatApp.Data.Repositories;
using ChatApp.Services.Interfaces;
using ChatApp.Services.Implementations;
using ChatApp.Entity;
using ChatApp.Hubs; 
using System.Security.Claims; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

builder.Services.AddSignalR();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "ChatApp.AuthCookie";
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

app.MapRazorPages();

await SeedDataAsync(app.Services);

app.Run();

async Task SeedDataAsync(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var announcementRepository = scope.ServiceProvider.GetRequiredService<IAnnouncementRepository>();

        await context.Database.MigrateAsync();

        User? adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");

        if (adminUser == null)
        {
            bool success = await authService.RegisterAsync(
                username: "admin",
                password: "123",
                firstName: "System",
                lastName: "Admin"
            );

            if (success)
            {
                adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                if (adminUser != null)
                {
                    adminUser.Role = "Admin";
                    context.Users.Update(adminUser);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Admin user 'admin' created successfully with password: 123");
                }
            }
        }


        if (adminUser != null && !await context.Announcements.AnyAsync())
        {
            var initialAnnouncement = new Announcement
            {
                Title = "Welcome to ChatApp!",
                Content = "This is the first official announcement from the Admin team.",
                CreatedByUserId = adminUser.Id, 
                Date = DateTime.Now.AddDays(-1)
            };

            await announcementRepository.AddAsync(initialAnnouncement);
            
            Console.WriteLine("Initial welcome announcement seeded successfully.");
        }
    }
}
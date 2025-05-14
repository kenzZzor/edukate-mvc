using EdukateMvc.Data;
using EdukateMvc.Models;
using EdukateMvc.Repositories;
using EdukateMvc.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// 1) Сервисы
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

// регистрация репозиториев и сервисов
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    const string adminRole = "Admin";
    const string adminEmail = "admin@example.com";
    const string adminPass = "Admin1!";

    // 1) Убедиться, что роль Admin существует
    if (!await roleMgr.RoleExistsAsync(adminRole))
    {
        var roleResult = await roleMgr.CreateAsync(new IdentityRole(adminRole));
        if (!roleResult.Succeeded)
        {
            throw new Exception(
                "Не удалось создать роль Admin: " +
                string.Join("; ", roleResult.Errors.Select(e => e.Description))
            );
        }
    }

    // 2) Убедиться, что пользователь не создан
    var adminUser = await userMgr.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
        var createResult = await userMgr.CreateAsync(adminUser, adminPass);
        if (!createResult.Succeeded)
        {
            throw new Exception(
                "Не удалось создать пользователя Admin: " +
                string.Join("; ", createResult.Errors.Select(e => e.Description))
            );
        }
    }

    // 3) Убедиться, что пользователь в роли Admin
    if (!await userMgr.IsInRoleAsync(adminUser, adminRole))
    {
        var addToRoleResult = await userMgr.AddToRoleAsync(adminUser, adminRole);
        if (!addToRoleResult.Succeeded)
        {
            throw new Exception(
                "Не удалось добавить пользователя в роль Admin: " +
                string.Join("; ", addToRoleResult.Errors.Select(e => e.Description))
            );
        }
    }
}



// 2) Конвейер обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

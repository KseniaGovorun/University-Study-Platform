using Microsoft.EntityFrameworkCore;
using UniversityStudyPlatform.DataAccess.Data;
using UniversityStudyPlatform.DataAccess.Repository;
using UniversityStudyPlatform.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using UniversityStudyPlatform.DataAccess.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Access/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
//    builder.Configuration.GetConnectionString("DefaultConnection")
//    ));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//builder.Services.AddScoped<IRepository<Course>, Repository<Course>>();
//builder.Services.AddScoped<IRepository<Shedule>, Repository<Shedule>>();
//builder.Services.AddScoped<IRepository<Student>, Repository<Student>>();
//builder.Services.AddScoped<IRepository<AccountBook>, Repository<AccountBook>>();
//builder.Services.AddScoped<IRepository<Teacher>, Repository<Teacher>>();
//builder.Services.AddScoped<IRepository<LoginData>, Repository<LoginData>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //something
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();

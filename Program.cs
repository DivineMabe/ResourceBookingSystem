using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResourceBookingSystem.Data; // Replace with your actual namespace

var builder = WebApplication.CreateBuilder(args);

// 🔹 Register services to the container
builder.Services.AddControllersWithViews();

// 🔹 Register Entity Framework Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Register your custom services here (if any)
// builder.Services.AddScoped<IMyService, MyService>();

var app = builder.Build();

// 🔹 Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ✅ This serves static files like CSS, JS, images, etc.
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// 🔹 Default MVC route: HomeController → Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

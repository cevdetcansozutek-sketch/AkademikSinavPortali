using Microsoft.EntityFrameworkCore;
using AkademikSinavPortali.Models; // Projenin adýný farklý koyduysan buradaki 'AkademikSinavPortali' kýsmýný kendi proje adýnla deðiþtir.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// BÝZÝM EKLEDÝÐÝMÝZ KISIM: Veritabaný servisini (DbContext) projeye tanýtýyoruz
builder.Services.AddDbContext<OnlineSinavDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
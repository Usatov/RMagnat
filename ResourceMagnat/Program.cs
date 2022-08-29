using Microsoft.EntityFrameworkCore;
using ResourceMagnat.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SessionDbContext>(o => o.UseInMemoryDatabase("Memory"), ServiceLifetime.Scoped);
builder.Services.AddDbContext<MainDbContext>(    o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
PrepareApplication.Init(app);

app.Run();

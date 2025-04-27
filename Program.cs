using Microsoft.EntityFrameworkCore;
using Bookstore.Context;
using Mapster;
using MapsterMapper;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Bookstore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var config = TypeAdapterConfig.GlobalSettings; 
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddDbContext<BookstoreContext>(
    optionsBuilder =>
        optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("BookstoreDb"))
        );
//builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMailService, MailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

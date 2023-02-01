using BookStore.Common.Settings;
using BookStore.Data;
using BookStore.WebFramework.Configuration;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var siteSetting = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

builder.Services.AddCustomIdentity(siteSetting.IdentitySettings);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

// Add services to the container.
builder.Services.AddMiniMvc();
 
builder.Services.AddJwtAuthentication(siteSetting.JwtSettings);

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();

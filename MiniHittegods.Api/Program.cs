using Microsoft.EntityFrameworkCore;
using MiniHittegods.Api.Data;
using MiniHittegods.Api.Repositories;
using MiniHittegods.Application.Interfaces;
using MiniHittegods.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<MiniHittegodsDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IFoundItemRepository, EfFoundItemRepository>();
builder.Services.AddScoped<FoundItemsService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MiniHittegodsDbContext>();
    db.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

public partial class Program { }
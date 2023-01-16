using BankAPI.Database;
using BankAPI.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OffersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Offers")));
builder.Services.AddScoped<OfferCommand>();
builder.Services.AddScoped<OfferQuery>();
builder.Services.AddScoped<ApplicationCommand>();
builder.Services.AddScoped<ApplicationQuery>();
builder.Services.AddScoped<DocumentService>();

builder.Services.AddControllers().
    AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
{
    using var context = serviceScope?.ServiceProvider.GetRequiredService<OffersDbContext>();
    context?.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
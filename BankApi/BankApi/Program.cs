using System.Text;
using BankAPI.Configuration;
using BankAPI.Database;
using BankAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<JwtTokenConfiguration>().
    Bind(builder.Configuration.GetSection(JwtTokenConfiguration.SectionName)).
    ValidateDataAnnotations();

// Add services to the container.
builder.Services.AddDbContext<OffersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Offers")));
builder.Services.AddScoped<OfferCommand>();
builder.Services.AddScoped<OfferQuery>();
builder.Services.AddScoped<ApplicationCommand>();
builder.Services.AddScoped<ApplicationQuery>();
builder.Services.AddScoped<DocumentService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers().
    AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
    });

var jwtConfig = builder.Configuration.GetSection(JwtTokenConfiguration.SectionName).Get<JwtTokenConfiguration>();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
        };
    });
builder.Services.AddAuthorization(options =>
{
    // TODO: RequireAuthenticatedUser() would be better but I can't get it to work :(
    options.FallbackPolicy = new AuthorizationPolicyBuilder().
        RequireRole(TokenService.UserRoleName).
        Build();
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

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    await using var context = serviceScope.ServiceProvider.GetRequiredService<OffersDbContext>();
    context.Database.Migrate();
    await context.EnsureAdminUserIsCreated();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
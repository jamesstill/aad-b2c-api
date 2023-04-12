using aad_b2c_api.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// configure AAD B2C security
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAdB2C", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
    options => { builder.Configuration.Bind("AzureAdB2C", options);  
});

// authorization claims policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("WeatherForecast", p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireClaim(ClaimTypes.Role, new[] { "WeatherForecastReader", "WeatherForecastWriter" });
    });

    options.AddPolicy("UserAdministration", p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireClaim(ClaimTypes.Role, new[] { "UserAdministrator" });
    });
});

// register claims transformation
builder.Services.AddScoped<IClaimsTransformation, TempUserRolesClaimsTransformation>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(c => c
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

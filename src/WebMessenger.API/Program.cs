using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using WebMessenger.Application;
using WebMessenger.Infrastructure;
using WebMessenger.Infrastructure.ClientConnection;
using WebMessenger.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowSpecificOrigin",
    policy =>
    {
      policy.WithOrigins("https://localhost:7151")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(c =>
   {
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebMessenger API", Version = "v1" });
     
     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
     {
       Description = "JWT Authorization header using the Bearer scheme.",
       Name = "Authorization",
       In = ParameterLocation.Header,
       Type = SecuritySchemeType.ApiKey,
       Scheme = "Bearer"
     });
   
     c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
       {
         new OpenApiSecurityScheme
         {
           Reference = new OpenApiReference
           {
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
           }
         },
         Array.Empty<string>()
       }
     });
   });

builder.Services.AddAuthentication(options =>
  {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = false,
      ValidateAudience = false,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };
  });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddSignalR();

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, _, configuration) =>
  configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<WebMessengerDbContext>();
db.Database.Migrate();

app.UseCors("AllowSpecificOrigin");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ClientHub>("/chat");

app.MapControllers();

app.Run();

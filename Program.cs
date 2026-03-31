using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

using scoring_Backend.Models.Admin;
using scoring_Backend.Models.Scoring;

using scoring_Backend.Repositories.Interfaces;
using scoring_Backend.Repositories.Implementations;

using scoring_Backend.Repositories.Interfaces.Evaluation;
using scoring_Backend.Repositories.Implementations.Evaluation;

using scoring_Backend.Repositories.Interfaces.Configuration;
using scoring_Backend.Repositories.Implementations.Configuration;

using scoring_Backend.Repositories;

// ✅ Ajoute ces namespaces pour Statistics
using scoring_Backend.Services;
using scoring_Backend.DTO.Statistics;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// JWT Authentication
// ===============================
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    context.Token = authHeader.Substring("Bearer ".Length).Trim();
                }

                Console.WriteLine("TOKEN EXTRACTED: [" + context.Token + "]");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT ERROR: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("JWT VALIDATED");
                return Task.CompletedTask;
            }
        };
    });

// ===============================
// Controllers + JSON
// ===============================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

// ===============================
// DbContexts
// ===============================
builder.Services.AddDbContext<SqrAdminContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqrAdmin")));

builder.Services.AddDbContext<SqrScoringContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqrScoring")));

// ===============================
// Swagger
// ===============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===============================
// CORS
// ===============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ===============================
// Services
// ===============================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

// ✅ Statistics Service
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

// ===============================
// Repositories
// ===============================
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRecordRepository, RecordRepository>();
builder.Services.AddScoped<IFilterRepository, FilterRepository>();
builder.Services.AddScoped<IViewConfigRepository, ViewConfigRepository>();
builder.Services.AddScoped<IConfigurationCampagnesRepository, ConfigurationCampagnesRepository>();
builder.Services.AddScoped<IAgentTeamRepository, AgentTeamRepository>();
builder.Services.AddScoped<IAgentMailConfigRepository, AgentMailConfigRepository>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
var app = builder.Build();

// ===============================
// Middleware
// ===============================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
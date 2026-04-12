using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // ✅ Ajouter
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer; // ✅ Ajouter cette ligne
using scoring_Backend.Models.Admin;
using scoring_Backend.Repositories.Interfaces;
using scoring_Backend.Repositories.Implementations;
using scoring_Backend.Models.Scoring;
using Microsoft.IdentityModel.Logging;
using scoring_Backend.Repositories.Interfaces.Evaluation;
using scoring_Backend.Repositories;
using scoring_Backend.Repositories.Implementations.Evaluation;
using scoring_Backend.Repositories.Interfaces.Configuration;
using scoring_Backend.Repositories.Implementations.Configuration;
using scoring_Backend.Repositories.Interfaces.Statistique;
using scoring_Backend.Repositories.Implementations.Statistique;
IdentityModelEventSource.ShowPII = true;
var builder = WebApplication.CreateBuilder(args);
/*builder.Services.AddHttpContextAccessor();

// Session (équivalent HttpContext.Current.Session de l'ancien code)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});*/
// ✅ JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        
        // 🔎 Debug JWT
       options.Events = new JwtBearerEvents
{
    OnMessageReceived = context =>
{
    // ✅ Lire depuis query string en priorité (pour les streams vidéo)
    var queryToken = context.Request.Query["token"].FirstOrDefault();
    if (!string.IsNullOrEmpty(queryToken))
    {
        context.Token = queryToken;
        Console.WriteLine("TOKEN FROM QUERY: [" + context.Token + "]");
        return Task.CompletedTask;
    }

    // Sinon lire depuis le header Authorization
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
// 🔹 Ajouter Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

// 🔹 Ajouter DbContext 
// ✅ Base SQR_Admin (authentification + users)
builder.Services.AddDbContext<SqrAdminContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqrAdmin")));
builder.Services.AddDbContext<SqrScoringContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqrScoring")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRecordRepository, RecordRepository>();
builder.Services.AddScoped<IViewConfigRepository,  ViewConfigRepository>();
builder.Services.AddScoped<IConfigurationCampagnesRepository, ConfigurationCampagnesRepository>();
builder.Services.AddScoped<IAgentTeamRepository, AgentTeamRepository>();
builder.Services.AddScoped<IAgentMailConfigRepository, AgentMailConfigRepository>();
builder.Services.AddScoped<IEvaluationListRepository, EvaluationListRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStatistiqueRepository, StatistiqueRepository>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
// Ajouter avec vos autres services
builder.Services.AddScoped<IUserDashboardRepository, UserDashboardRepository>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Services.AddDbContext<SqrScoringContext>(options =>
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("SqrScoring"))
        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
               LogLevel.Information)
        .EnableSensitiveDataLogging()
);
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();   // ← MANQUANT — ajouter cette ligne
//app.UseSession(); // Activer les sessions
app.UseAuthorization();

app.MapControllers();

app.Run();
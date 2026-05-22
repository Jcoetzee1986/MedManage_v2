using FluentValidation;
using MedManage.Infrastructure.Persistence;
using MedManage.Infrastructure.Services;
using MedManage.Infrastructure;
using MedManage.Core.Interfaces;
using MedManage.Core.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog - reads all configuration from appsettings.json
// This allows full control via appsettings and environment variables
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Add HttpContextAccessor for accessing current user
builder.Services.AddHttpContextAccessor();

// Register current user service
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Configure Email Settings with environment variable override support
builder.Services.Configure<EmailSettings>(options =>
{
    var emailSection = builder.Configuration.GetSection("EmailSettings");
    emailSection.Bind(options);
    
    // Override with environment variables if present
    var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
    var smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT");
    var smtpUsername = Environment.GetEnvironmentVariable("SMTP_USERNAME");
    var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
    var fromEmail = Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL");
    
    if (!string.IsNullOrEmpty(smtpHost)) options.SmtpHost = smtpHost;
    if (!string.IsNullOrEmpty(smtpPort) && int.TryParse(smtpPort, out var port)) options.SmtpPort = port;
    if (!string.IsNullOrEmpty(smtpUsername)) options.SmtpUsername = smtpUsername;
    if (!string.IsNullOrEmpty(smtpPassword)) options.SmtpPassword = smtpPassword;
    if (!string.IsNullOrEmpty(fromEmail)) options.FromEmail = fromEmail;
});

// Register email service
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure DbContext
builder.Services.AddDbContext<MedManageDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    );
    
    // Enable detailed logging for development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Configure CORS
var corsOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:4200" };
var allowCredentials = builder.Configuration.GetValue<bool>("CorsSettings:AllowCredentials", true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(corsOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
        
        if (allowCredentials)
        {
            policy.AllowCredentials();
        }
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MedManage API",
        Version = "v1",
        Description = "Medical Case Management System API"
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
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

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<MedManage.Core.Validators.CreateMemberRequestValidator>();

// Register Infrastructure (AutoMapper, Repositories, Services)
builder.Services.AddInfrastructure();

// Register auth service (not part of business services)
builder.Services.AddScoped<IAuthService, AuthService>();

// Configure Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MedManageDbContext>(
        name: "database",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
        tags: new[] { "db", "sql", "sqlserver" }
    );

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MedManage API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Map health check endpoint
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds,
                exception = e.Value.Exception?.Message
            })
        });
        await context.Response.WriteAsync(result);
    }
});

Log.Information("MedManage API Starting...");

app.Run();

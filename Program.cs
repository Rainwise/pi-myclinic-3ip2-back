using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using myclinic_back.Interfaces;
using myclinic_back.Models;
using myclinic_back.Services;
using myclinic_back.Utilities;
using myclinic_back.Utilities.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1",
        new OpenApiInfo { Title = "myclinic-back API", Version = "v1" });

    option.AddSecurityDefinition("bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter JWT token (without the 'Bearer ' prefix)",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

    option.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = new List<string>()
    });
});



var secureKey = builder.Configuration["JWT:SecureKey"];
if (string.IsNullOrWhiteSpace(secureKey))
{
    throw new InvalidOperationException("JWT:SecureKey is not configured in appsettings.json or user secrets.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        var keyBytes = Encoding.UTF8.GetBytes(secureKey);
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<PiProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ILogEventPublisher, LogEventPublisher>();
builder.Services.AddSingleton<LoggerService>();

builder.Services.AddSingleton<ICrudLogger>(sp =>
{
    var logger = sp.GetRequiredService<LoggerService>();
    var http = sp.GetRequiredService<IHttpContextAccessor>();
    return new AdminLoggerDecorator(logger, http);
});


//builder.Services.AddSingleton<IAuditLogger, AuditLogger>();

//builder.Services.AddSingleton<ICrudLogger, LoggerService>();
builder.Services.AddSingleton<FileObserver>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<EmailObserver>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddAuthorization();

var app = builder.Build();

var publisher = app.Services.GetRequiredService<ILogEventPublisher>();
publisher.Subscribe(app.Services.GetRequiredService<FileObserver>());
publisher.Subscribe(app.Services.GetRequiredService<EmailObserver>());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

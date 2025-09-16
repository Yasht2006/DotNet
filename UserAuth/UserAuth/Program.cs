using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UserAuth.Application.DTOs.User;
using UserAuth.Application.Mapping;
using UserAuth.Infrastructure.Context;
using UserAuth.Infrastructure.Repositories.Implementation;
using UserAuth.Infrastructure.Repositories.Interfaces;
using UserAuth.Infrastructure.Services.Implementation;
using UserAuth.Infrastructure.Services.Interfaces;
using UserAuth.Infrastructure.Validators.User;
using UserAuth.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor(); // ?? Register IHttpContextAccessor


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DBContext
string? ConnectionString = builder.Configuration.GetConnectionString("PGConnection");
builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseNpgsql(ConnectionString);
});

// Register Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<UserCreateDTO>, UserCreateDTOValidator>();
builder.Services.AddScoped<IValidator<LoginDTO>, LoginDTOValidator>();

// Register Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenBlacklistService, TokenBlacklistService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Angular dev server
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new Exception("JWT settings are not properly configured in appsettings.json");
}

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BankingSystem API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' [space] followed by your JWT token"
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



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<TokenBlacklistMiddleware>();


app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");



app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

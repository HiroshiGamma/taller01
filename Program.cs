using System.Text;
using api.src.data;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using taller01.src.Helpers;
using taller01.src.Interface;
using api.src.Interface;
using api.src.Repository;
using taller01.src.models;
using taller01.src.Repository;
using taller01.src.Service;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var cloudName = Environment.GetEnvironmentVariable("CloudinaryName");
var apiKey = Environment.GetEnvironmentVariable("ApiKey");
var apiSecret = Environment.GetEnvironmentVariable("ApiSecret");


if (cloudName == null || apiKey == null || apiSecret == null)
{
    throw new Exception("Cloudinary settings not found in environment variables.");
}

var cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
var cloudinary = new Cloudinary(cloudinaryAccount);

builder.Services.AddSingleton(cloudinary);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor(); 

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options => 
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = 
        options.DefaultChallengeScheme = 
        options.DefaultForbidScheme =
        options.DefaultScheme = 
        options.DefaultSignInScheme = 
        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(option => {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "")),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source=app.db";
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(connectionString));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:4200") 
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); 
        });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();
    Seeders.Initialize(services);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

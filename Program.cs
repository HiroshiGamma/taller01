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
using taller01.src.Interfaces;
using taller01.src.models;
using taller01.src.Repository;
using taller01.src.Service;

var builder = WebApplication.CreateBuilder(args);


Env.Load();

var issuer =Environment.GetEnvironmentVariable("Issuer");
var audience =Environment.GetEnvironmentVariable("Audience");
var signingKey =Environment.GetEnvironmentVariable("SigningKey");

var cloudinaryName = Environment.GetEnvironmentVariable("CloudinaryName");
var cloudinaryKey = Environment.GetEnvironmentVariable("ApiKey");
var cloudinarySecret = Environment.GetEnvironmentVariable("ApiSecret");
    var cloudinaryAccount = new Account(
        cloudinaryName,
        cloudinaryKey,
        cloudinarySecret
    );
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(
    opt => 
    {
        opt.Password.RequireDigit = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 8;
    }
).AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme =
    opt.DefaultChallengeScheme =
    opt.DefaultForbidScheme = 
    opt.DefaultScheme =
    opt.DefaultSignInScheme =
    opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey ?? throw new ArgumentNullException(signingKey))),
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

string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Data Source-app.db";
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(connectionString));
builder.Services.AddControllers();
var app = builder.Build();  

using ( var scope = app.Services.CreateScope()){

    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();
    Seeders.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Run();

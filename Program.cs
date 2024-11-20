using api.src.data;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using taller01.src.Helpers;
using taller01.src.Interfaces;
using taller01.src.Repository;

var builder = WebApplication.CreateBuilder(args);


Env.Load();

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
builder.Services.AddScoped<IUserRepository, UserRepository>();

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
app.Run();

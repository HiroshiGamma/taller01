using api.src.data;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using taller01.src.Helpers;

var builder = WebApplication.CreateBuilder(args);


Env.Load();

var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
    var cloudinaryAccount = new Account(
        cloudinarySettings!.CloudName,
        cloudinarySettings.ApiKey,
        cloudinarySettings.ApiSecret
    );
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

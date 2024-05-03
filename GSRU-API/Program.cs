using DotNetEnv;
using GSRU_Common.Extensions;
using GSRU_API.BusinessLayer;
using GSRU_API.Common.Settings;
using GSRU_API.Extensions;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
IConfiguration appSettingsValue = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsValue);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEncryptionService();
builder.Services.RegisterBusinessLayerDependencies();
builder.Services.AddCustomSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

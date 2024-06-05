using DotNetEnv;
using GSRU_Common.Extensions;
using GSRU_API.BusinessLayer;
using GSRU_API.Common.Settings;
using GSRU_API.Extensions;
using GSRU_API.Filters;
using GSRU_Common.Models;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
IConfiguration appSettingsValue = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsValue);
var appSettings = appSettingsValue.Get<AppSettings>();
ArgumentNullException.ThrowIfNull(appSettings);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<RoleAuthorizationFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new WorkloadConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEncryptionService();
builder.Services.RegisterBusinessLayerDependencies();
builder.Services.AddCustomSwaggerGen();
builder.Services.AddCorsOrigins(appSettings.CorsOrigins);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using HashBack.Services;
using HashBack.Helpers;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(".env");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddSingleton<IVerificationCacheService, VerificationCacheService>();

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

app.Run($"http://*:{(app.Environment.IsDevelopment() ? "5101" : "80")}");
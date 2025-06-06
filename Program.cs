using LancamentosQuattroCoffe.Service.Lancamento;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080); // ou Listen(IPAddress.Any, 8080)
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IDbConnection>(sp =>
    new Npgsql.NpgsqlConnection(
        "Host=restless-sunset-5689.flycast;Port=5432;Username=postgres;Password=QDNe6rHFzwbw2X9;Database=postgres;Ssl Mode=Allow;Trust Server Certificate=true;"
    ));
builder.Services.AddScoped<ILancamentoService, LancamentoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("PermitirTudo");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API de Lançamentos funcionando!");

if (!app.Environment.IsDevelopment())
{
    app.Run("http://0.0.0.0:80");
}
else
{
    app.Run();
}



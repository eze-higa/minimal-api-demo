
using minimal_api_demo.Data;
using minimal_api_demo.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DemoContext") ?? "Data Source=MinimalApiDemoDB.db";
builder.Services.AddSqlite<AppDbContext>(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();


app.AddHomeEndpoint();

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";

app.Logger.LogInformation($"Listening in port {port}");
app.Run($"https://localhost:{port}");

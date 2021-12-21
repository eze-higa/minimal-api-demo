
using Microsoft.EntityFrameworkCore;
using minimal_api_demo.Data;
using minimal_api_demo.Endpoints;
using minimal_api_demo.Entities;

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

app.MapFallback(() => Results.Redirect("/swagger"));

app.AddHomeEndpoint();

app.MapGet("/todos", async (AppDbContext dbContext) => 
{
    return await dbContext.ToDoItems.ToListAsync();
});

app.MapGet("/todos/{id}", async (AppDbContext dbContext, int id) => 
{ 
    var todoItem = await dbContext.ToDoItems.FindAsync(id);
    if(todoItem != null)
        return Results.Ok(todoItem);

    return Results.NotFound();
});

app.MapPost("/todos", async (AppDbContext dbContext, ToDoItem toDo) =>
{ 
    await dbContext.ToDoItems.AddAsync(toDo);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/todos/{toDo.Id}", toDo);
});

app.MapPut("/todos/{id}", async (AppDbContext dbContext, int id, ToDoItem toDo) => 
{
    if(id != toDo.Id)
        return Results.BadRequest();

    if (!await dbContext.ToDoItems.AnyAsync(t => t.Id == id))
        return Results.NotFound();

    dbContext.ToDoItems.Update(toDo);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("/todos/id", async (AppDbContext dbContext, int id) => 
{
    var todoItem = await dbContext.ToDoItems.FindAsync(id);
    
    if(todoItem == null)
        return Results.NotFound();

    dbContext.ToDoItems.Remove(todoItem);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";

app.Logger.LogInformation($"Listening in port {port}");
app.Run($"https://localhost:{port}");

using DotNet8.Packages.GridifyExample.MinimalApi.AppDbContexts;
using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
}, ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/greeting", () =>
{
    return Results.Ok("Hello!");
});

app.MapGet("/blog", async (int pageNo, int pageSize, AppDbContext db) =>
{
    var query = new GridifyQuery()
    {
        Page = pageNo,
        PageSize = pageSize
    };
    return Results.Ok(await db.Tbl_Blogs.GridifyAsync(query));
});

app.MapGet("/blogV1", async (int pageNo, int pageSize, AppDbContext db) =>
{
    var query = db.Tbl_Blogs.AsNoTracking()
    .OrderByDescending(x => x.BlogId);

    return Results.Ok(await query.GridifyAsync(new GridifyQuery()
    {
        Page = pageNo,
        PageSize = pageSize
    }));
});

app.UseHttpsRedirection();

app.Run();

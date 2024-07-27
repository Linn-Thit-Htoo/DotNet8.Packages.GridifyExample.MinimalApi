using DotNet8.Packages.GridifyExample.MinimalApi.AppDbContexts;
using Gridify;
using Gridify.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
    },
    ServiceLifetime.Transient
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet(
    "/blog",
    async (int pageNo, int pageSize, AppDbContext db) =>
    {
        var query = new GridifyQuery() { Page = pageNo, PageSize = pageSize };
        return Results.Ok(await db.Tbl_Blogs.GridifyAsync(query));
    }
);

app.MapGet(
    "/blogV1",
    async (int pageNo, int pageSize, AppDbContext db) =>
    {
        var query = db.Tbl_Blogs.AsNoTracking().OrderByDescending(x => x.BlogId);

        return Results.Ok(
            await query.GridifyAsync(new GridifyQuery() { Page = pageNo, PageSize = pageSize })
        );
    }
);

app.MapGet(
    "/blogV2",
    async (int pageNo, int pageSize, AppDbContext db) =>
    {
        var query = new GridifyQuery() { Page = pageNo, PageSize = pageSize, OrderBy = "BlogId desc" };
        return Results.Ok(await db.Tbl_Blogs.GridifyAsync(query));
    }
);

app.UseHttpsRedirection();

app.Run();

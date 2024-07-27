namespace DotNet8.Packages.GridifyExample.MinimalApi.AppDbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Tbl_Blog> Tbl_Blogs { get; set; }
}

using Microsoft.EntityFrameworkCore;
using SpaAngular.Models;

namespace SpaAngular.Data;

public class ApplicationDbContext : DbContext {

    public DbSet<User> Users { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
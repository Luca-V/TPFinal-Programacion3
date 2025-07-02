using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_Programacion3.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Client> Clients { get; set; }
        public DbSet<Movement> Movements { get; set; }
    }
}

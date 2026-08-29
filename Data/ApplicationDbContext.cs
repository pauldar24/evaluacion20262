using Microsoft.EntityFrameworkCore;
using evaluacion20262.Models;

namespace evaluacion20262.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SolicitudServicio> SolicitudesServicio { get; set; }
}
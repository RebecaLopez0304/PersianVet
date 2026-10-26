using Microsoft.EntityFrameworkCore;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Contexto de Entity Framework Core (Database First) para PersianVetDB.
/// Las mismas clases de Modelos se usan como entidades: una clase por tabla.
/// </summary>
public class PersianVetContext : DbContext
{
    public PersianVetContext(DbContextOptions<PersianVetContext> opciones) : base(opciones)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Veterinario> Veterinarios => Set<Veterinario>();
    public DbSet<Mascota> Mascotas => Set<Mascota>();
    public DbSet<Cita> Citas => Set<Cita>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();
    public DbSet<Vacuna> Vacunas => Set<Vacuna>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // EF Core ignora por convención las propiedades de solo lectura (NombreCompleto, Propietario...).
        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("Clientes");
            e.HasKey(x => x.IdCliente);
            e.Property(x => x.FechaRegistro).HasColumnType("date");
        });

        modelBuilder.Entity<Veterinario>(e =>
        {
            e.ToTable("Veterinarios");
            e.HasKey(x => x.IdVeterinario);
        });

        modelBuilder.Entity<Mascota>(e =>
        {
            e.ToTable("Mascotas");
            e.HasKey(x => x.IdMascota);
            e.Property(x => x.FechaNacimiento).HasColumnType("date");
            e.Property(x => x.Peso).HasColumnType("decimal(6,2)");
            e.Property(x => x.Observaciones).IsRequired(false);
            e.HasOne(x => x.Cliente).WithMany(c => c.Mascotas).HasForeignKey(x => x.IdCliente);
        });

        modelBuilder.Entity<Cita>(e =>
        {
            e.ToTable("Citas");
            e.HasKey(x => x.IdCita);
            e.HasOne(x => x.Mascota).WithMany(m => m.Citas).HasForeignKey(x => x.IdMascota);
            e.HasOne(x => x.Veterinario).WithMany(v => v.Citas).HasForeignKey(x => x.IdVeterinario);
        });

        modelBuilder.Entity<Consulta>(e =>
        {
            e.ToTable("Consultas");
            e.HasKey(x => x.IdConsulta);
            e.Property(x => x.Observaciones).IsRequired(false);
            e.HasOne(x => x.Mascota).WithMany(m => m.Consultas).HasForeignKey(x => x.IdMascota);
            e.HasOne(x => x.Veterinario).WithMany(v => v.Consultas).HasForeignKey(x => x.IdVeterinario);
            e.HasMany(x => x.ListaTratamientos).WithOne().HasForeignKey(t => t.IdConsulta);
        });

        modelBuilder.Entity<Tratamiento>(e =>
        {
            e.ToTable("Tratamientos");
            e.HasKey(x => x.IdTratamiento);
            e.Property(x => x.Indicaciones).IsRequired(false);
        });

        modelBuilder.Entity<Vacuna>(e =>
        {
            e.ToTable("Vacunas");
            e.HasKey(x => x.IdVacuna);
            e.Property(x => x.FechaAplicacion).HasColumnType("date");
            e.Property(x => x.ProximaDosis).HasColumnType("date");
            e.HasOne(x => x.Mascota).WithMany(m => m.Vacunas).HasForeignKey(x => x.IdMascota);
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("Usuarios");
            e.HasKey(x => x.IdUsuario);
        });
    }
}

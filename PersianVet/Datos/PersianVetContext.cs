using Microsoft.EntityFrameworkCore;
using PersianVet.Entidades;

namespace PersianVet.Datos;

/// <summary>
/// Contexto de Entity Framework Core (Database First) para PersianVetDB.
/// </summary>
public class PersianVetContext : DbContext
{
    public PersianVetContext(DbContextOptions<PersianVetContext> opciones) : base(opciones)
    {
    }

    public DbSet<ClienteEntidad> Clientes => Set<ClienteEntidad>();
    public DbSet<VeterinarioEntidad> Veterinarios => Set<VeterinarioEntidad>();
    public DbSet<MascotaEntidad> Mascotas => Set<MascotaEntidad>();
    public DbSet<CitaEntidad> Citas => Set<CitaEntidad>();
    public DbSet<ConsultaEntidad> Consultas => Set<ConsultaEntidad>();
    public DbSet<TratamientoEntidad> Tratamientos => Set<TratamientoEntidad>();
    public DbSet<VacunaEntidad> Vacunas => Set<VacunaEntidad>();
    public DbSet<UsuarioEntidad> Usuarios => Set<UsuarioEntidad>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClienteEntidad>(e =>
        {
            e.ToTable("Clientes");
            e.HasKey(x => x.IdCliente);
            e.Property(x => x.Nombres).HasMaxLength(80);
            e.Property(x => x.Apellidos).HasMaxLength(80);
            e.Property(x => x.Telefono).HasMaxLength(20);
            e.Property(x => x.Direccion).HasMaxLength(200);
            e.Property(x => x.Correo).HasMaxLength(120);
            e.Property(x => x.FechaRegistro).HasColumnType("date");
            e.HasIndex(x => x.Correo).IsUnique();
        });

        modelBuilder.Entity<VeterinarioEntidad>(e =>
        {
            e.ToTable("Veterinarios");
            e.HasKey(x => x.IdVeterinario);
            e.Property(x => x.Nombres).HasMaxLength(80);
            e.Property(x => x.Apellidos).HasMaxLength(80);
            e.Property(x => x.Especialidad).HasMaxLength(100);
            e.Property(x => x.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<MascotaEntidad>(e =>
        {
            e.ToTable("Mascotas");
            e.HasKey(x => x.IdMascota);
            e.Property(x => x.Nombre).HasMaxLength(80);
            e.Property(x => x.Especie).HasMaxLength(40);
            e.Property(x => x.Raza).HasMaxLength(80);
            e.Property(x => x.Sexo).HasMaxLength(10);
            e.Property(x => x.Color).HasMaxLength(80);
            e.Property(x => x.Observaciones).HasMaxLength(500);
            e.Property(x => x.FechaNacimiento).HasColumnType("date");
            e.Property(x => x.Peso).HasColumnType("decimal(6,2)");
            e.HasOne(x => x.Cliente).WithMany(c => c.Mascotas).HasForeignKey(x => x.IdCliente);
        });

        modelBuilder.Entity<CitaEntidad>(e =>
        {
            e.ToTable("Citas");
            e.HasKey(x => x.IdCita);
            e.Property(x => x.FechaHora).HasColumnType("datetime2(0)");
            e.Property(x => x.Motivo).HasMaxLength(200);
            e.Property(x => x.Estado).HasMaxLength(20);
            e.HasOne(x => x.Mascota).WithMany(m => m.Citas).HasForeignKey(x => x.IdMascota);
            e.HasOne(x => x.Veterinario).WithMany(v => v.Citas).HasForeignKey(x => x.IdVeterinario);
        });

        modelBuilder.Entity<ConsultaEntidad>(e =>
        {
            e.ToTable("Consultas");
            e.HasKey(x => x.IdConsulta);
            e.Property(x => x.FechaHora).HasColumnType("datetime2(0)");
            e.Property(x => x.Motivo).HasMaxLength(200);
            e.Property(x => x.Diagnostico).HasMaxLength(500);
            e.Property(x => x.Observaciones).HasMaxLength(500);
            e.HasOne(x => x.Mascota).WithMany(m => m.Consultas).HasForeignKey(x => x.IdMascota);
            e.HasOne(x => x.Veterinario).WithMany(v => v.Consultas).HasForeignKey(x => x.IdVeterinario);
        });

        modelBuilder.Entity<TratamientoEntidad>(e =>
        {
            e.ToTable("Tratamientos");
            e.HasKey(x => x.IdTratamiento);
            e.Property(x => x.Medicamento).HasMaxLength(120);
            e.Property(x => x.Dosis).HasMaxLength(80);
            e.Property(x => x.Frecuencia).HasMaxLength(100);
            e.Property(x => x.Duracion).HasMaxLength(80);
            e.Property(x => x.Indicaciones).HasMaxLength(300);
            e.HasOne(x => x.Consulta).WithMany(c => c.Tratamientos).HasForeignKey(x => x.IdConsulta);
        });

        modelBuilder.Entity<VacunaEntidad>(e =>
        {
            e.ToTable("Vacunas");
            e.HasKey(x => x.IdVacuna);
            e.Property(x => x.NombreVacuna).HasMaxLength(120);
            e.Property(x => x.FechaAplicacion).HasColumnType("date");
            e.Property(x => x.ProximaDosis).HasColumnType("date");
            e.Property(x => x.Lote).HasMaxLength(60);
            e.HasOne(x => x.Mascota).WithMany(m => m.Vacunas).HasForeignKey(x => x.IdMascota);
        });

        modelBuilder.Entity<UsuarioEntidad>(e =>
        {
            e.ToTable("Usuarios");
            e.HasKey(x => x.IdUsuario);
            e.Property(x => x.NombreUsuario).HasMaxLength(50);
            e.Property(x => x.ClaveHash).HasColumnType("char(64)");
            e.Property(x => x.Rol).HasMaxLength(40);
            e.HasIndex(x => x.NombreUsuario).IsUnique();
        });
    }
}

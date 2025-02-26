using Microsoft.EntityFrameworkCore;
using StringHub.Models;

namespace StringHub.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Raqueta> Raquetas { get; set; } = null!;
        public DbSet<Cuerda> Cuerdas { get; set; } = null!;
        public DbSet<Servicio> Servicios { get; set; } = null!;
        public DbSet<OrdenEncordado> OrdenesEncordado { get; set; } = null!;
        public DbSet<HistorialTension> HistorialTensiones { get; set; } = null!;
        public DbSet<Disponibilidad> Disponibilidades { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(e => e.UsuarioId);
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Contraseña).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Apellido).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.TipoUsuario).HasMaxLength(20).IsRequired();
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UltimaModificacion).HasDefaultValueSql("GETDATE()");
            });

            // Configuración para Raqueta
            modelBuilder.Entity<Raqueta>(entity =>
            {
                entity.ToTable("Raquetas");
                entity.HasKey(e => e.RaquetaId);
                entity.Property(e => e.Marca).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Modelo).HasMaxLength(50).IsRequired();
                entity.Property(e => e.NumeroSerie).HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasColumnType("TEXT");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Cuerda
            modelBuilder.Entity<Cuerda>(entity =>
            {
                entity.ToTable("Cuerdas");
                entity.HasKey(e => e.CuerdaId);
                entity.Property(e => e.Marca).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Modelo).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Calibre).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Material).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Color).HasMaxLength(30);
                entity.Property(e => e.Precio).HasColumnType("DECIMAL(10,2)").IsRequired();
                entity.Property(e => e.Stock).HasDefaultValue(0);
                entity.Property(e => e.Activo).HasDefaultValue(true);
            });

            // Configuración para Servicio
            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("Servicios");
                entity.HasKey(e => e.ServicioId);
                entity.Property(e => e.NombreServicio).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Descripcion).HasColumnType("TEXT");
                entity.Property(e => e.PrecioBase).HasColumnType("DECIMAL(10,2)").IsRequired();
                entity.Property(e => e.TiempoEstimado).IsRequired();
                entity.Property(e => e.Activo).HasDefaultValue(true);
            });

            // Configuración para OrdenEncordado
            modelBuilder.Entity<OrdenEncordado>(entity =>
            {
                entity.ToTable("OrdenesEncordado");
                entity.HasKey(e => e.OrdenId);
                entity.Property(e => e.TensionVertical).HasColumnType("DECIMAL(4,1)").IsRequired();
                entity.Property(e => e.TensionHorizontal).HasColumnType("DECIMAL(4,1)");
                entity.Property(e => e.Estado).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Comentarios).HasColumnType("TEXT");
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.PrecioTotal).HasColumnType("DECIMAL(10,2)").IsRequired();

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.UsuarioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Raqueta>()
                    .WithMany()
                    .HasForeignKey(e => e.RaquetaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Servicio>()
                    .WithMany()
                    .HasForeignKey(e => e.ServicioId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Cuerda>()
                    .WithMany()
                    .HasForeignKey(e => e.CuerdaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.EncordadorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para HistorialTension
            modelBuilder.Entity<HistorialTension>(entity =>
            {
                entity.ToTable("HistorialTensiones");
                entity.HasKey(e => e.HistorialId);
                entity.Property(e => e.TensionVertical).HasColumnType("DECIMAL(4,1)").IsRequired();
                entity.Property(e => e.TensionHorizontal).HasColumnType("DECIMAL(4,1)");
                entity.Property(e => e.Fecha).HasDefaultValueSql("GETDATE()");

                entity.HasOne<Raqueta>()
                    .WithMany()
                    .HasForeignKey(e => e.RaquetaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<OrdenEncordado>()
                    .WithMany()
                    .HasForeignKey(e => e.OrdenId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Cuerda>()
                    .WithMany()
                    .HasForeignKey(e => e.CuerdaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración para Disponibilidad
            modelBuilder.Entity<Disponibilidad>(entity =>
            {
                entity.ToTable("Disponibilidad");
                entity.HasKey(e => e.DisponibilidadId);
                entity.Property(e => e.DiaSemana).IsRequired();
                entity.Property(e => e.HoraInicio).IsRequired();
                entity.Property(e => e.HoraFin).IsRequired();

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.EncordadorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
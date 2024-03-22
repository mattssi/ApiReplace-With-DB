using Microsoft.EntityFrameworkCore;

namespace ChistesAPI_DB.Models
{
    public partial class ApiReplaceContext : DbContext
    {
        public DbSet<Chiste> Chistes { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        public ApiReplaceContext()
        {
        }

        public ApiReplaceContext(DbContextOptions<ApiReplaceContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chiste>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__CHISTES__3214EC07B3581AF0");
                entity.ToTable("CHISTES");
                entity.Property(e => e.Contenido)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("contenido");
            });

            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.HasKey(e => e.IdEstudiante).HasName("PK__Estudian__B5007C247BB20697");
                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07B3581AF0");
                entity.ToTable("Usuarios");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Password");

                // Otras configuraciones de propiedades si las tienes
            });

            // Configuración de la entidad Usuario


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChistesAPI_DB.Models
{
    public partial class ApiReplaceContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiReplaceContext(DbContextOptions<ApiReplaceContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Chiste> Chistes { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<AuditTables> AuditTables { get; set; }

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
                entity.Property(e => e.FechaNacimiento).HasColumnType("date");
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
            });

            modelBuilder.Entity<AuditTables>(entity =>
            {
                entity.HasKey(e => e.IdAudit);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList(); // Convertir la colección en una lista

            var usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";

            foreach (var entry in modifiedEntities)
            {
                var tableName = entry.Metadata?.GetTableName() ?? "Unknown";

                var auditEntry = new AuditTables
                {
                    Tipo = entry.State.ToString(),
                    Tabla = tableName,
                    Fecha = DateTime.Now,
                    Usuario = usuario
                };

                if (entry.Entity is Chiste chiste)
                {
                    auditEntry.Registro = chiste.Id;
                    auditEntry.Campo = "Contenido";
                    auditEntry.ValorDespues = chiste.Contenido;

                    // Si es una modificación, busca el valor anterior en la base de datos
                    if (entry.State == EntityState.Modified)
                    {
                        var originalChiste = this.Chistes.AsNoTracking().FirstOrDefault(c => c.Id == chiste.Id);
                        auditEntry.ValorAntes = originalChiste?.Contenido;
                    }
                }
                else if (entry.Entity is Estudiante estudiante)
                {
                    auditEntry.Registro = estudiante.IdEstudiante;
                    auditEntry.Campo = "Nombre, Apellido, FechaNacimiento";
                    auditEntry.ValorDespues = $"Nombre: {estudiante.Nombre}, Apellido: {estudiante.Apellido}, FechaNacimiento: {estudiante.FechaNacimiento}";

                    // Si es una modificación, busca el valor anterior en la base de datos
                    if (entry.State == EntityState.Modified)
                    {
                        var originalEstudiante = this.Estudiantes.AsNoTracking().FirstOrDefault(e => e.IdEstudiante == estudiante.IdEstudiante);
                        auditEntry.ValorAntes = $"Nombre: {originalEstudiante?.Nombre}, Apellido: {originalEstudiante?.Apellido}, FechaNacimiento: {originalEstudiante?.FechaNacimiento}";
                    }
                }

                AuditTables.Add(auditEntry); // Agrega la entrada de auditoría al DbSet correspondiente
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);

            var usuario = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";

            // Lista para almacenar las entradas de auditoría
            var auditEntries = new List<AuditTables>();

            foreach (var entry in modifiedEntities)
            {
                var tableName = entry.Metadata?.GetTableName() ?? "Unknown";

                var auditEntry = new AuditTables
                {
                    Tipo = entry.State.ToString(),
                    Tabla = tableName,
                    Fecha = DateTime.Now,
                    Usuario = usuario
                };

                if (entry.Entity is Chiste chiste)
                {
                    auditEntry.Registro = chiste.Id;
                    auditEntry.Campo = "Contenido";
                    auditEntry.ValorDespues = chiste.Contenido;

                    // Si es una modificación, busca el valor anterior en la base de datos
                    if (entry.State == EntityState.Modified)
                    {
                        var originalChiste = this.Chistes.AsNoTracking().FirstOrDefault(c => c.Id == chiste.Id);
                        auditEntry.ValorAntes = originalChiste?.Contenido;
                    }
                }
                else if (entry.Entity is Estudiante estudiante)
                {
                    auditEntry.Registro = estudiante.IdEstudiante;
                    auditEntry.Campo = "Nombre, Apellido, FechaNacimiento";
                    auditEntry.ValorDespues = $"Nombre: {estudiante.Nombre}, Apellido: {estudiante.Apellido}, FechaNacimiento: {estudiante.FechaNacimiento}";

                    // Si es una modificación, busca el valor anterior en la base de datos
                    if (entry.State == EntityState.Modified)
                    {
                        var originalEstudiante = this.Estudiantes.AsNoTracking().FirstOrDefault(e => e.IdEstudiante == estudiante.IdEstudiante);
                        auditEntry.ValorAntes = $"Nombre: {originalEstudiante?.Nombre}, Apellido: {originalEstudiante?.Apellido}, FechaNacimiento: {originalEstudiante?.FechaNacimiento}";
                    }
                }

                auditEntries.Add(auditEntry); // Agrega la entrada de auditoría a la lista
            }

            // Agrega todas las entradas de auditoría a la colección AuditTables
            AuditTables.AddRange(auditEntries);

            // Guarda los cambios en la base de datos
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}




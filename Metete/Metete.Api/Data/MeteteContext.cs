using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Data;

public partial class MeteteContext : DbContext
{
    public MeteteContext(DbContextOptions<MeteteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminNotificacion> AdminNotificaciones { get; set; }

    public virtual DbSet<CategoriaGenero> CategoriaGeneros { get; set; }

    public virtual DbSet<CentroDeporte> CentroDeportes { get; set; }

    public virtual DbSet<Comuna> Comunas { get; set; }

    public virtual DbSet<CriterioMvp> CriterioMvps { get; set; }

    public virtual DbSet<DetalleFuncionalidad> DetalleFuncionalidades { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Funcionalidad> Funcionalidades { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Nacionalidad> Nacionalidades { get; set; }

    public virtual DbSet<Notificacion> Notificaciones { get; set; }

    public virtual DbSet<Pais> Paises { get; set; }

    public virtual DbSet<Region> Regiones { get; set; }

    public virtual DbSet<Rol> Roles { get; set; }

    public virtual DbSet<TipoDeporte> TipoDeportes { get; set; }

    public virtual DbSet<TipoDivisa> TipoDivisas { get; set; }

    public virtual DbSet<TipoGenero> TipoGeneros { get; set; }

    public virtual DbSet<TipoMembresia> TipoMembresias { get; set; }

    public virtual DbSet<TipoNotificacion> TipoNotificaciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioComuna> UsuarioComunas { get; set; }

    public virtual DbSet<UsuarioEvaluacion> UsuarioEvaluaciones { get; set; }

    public virtual DbSet<UsuarioEvento> UsuarioEventos { get; set; }

    public virtual DbSet<UsuarioHorario> UsuarioHorarios { get; set; }

    public virtual DbSet<UsuarioTipoDeporte> UsuarioTipoDeportes { get; set; }

    public virtual DbSet<UsuarioTipoNotificacion> UsuarioTipoNotificaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminNotificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("admin_notificacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.Mensaje).HasMaxLength(500);
            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        modelBuilder.Entity<CategoriaGenero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria_genero");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<CentroDeporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("centro_deporte");

            entity.HasIndex(e => e.IdComuna, "fk_centro_deporte_comuna_idx");

            entity.Property(e => e.Latitud).HasPrecision(9, 6);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.Longitud).HasPrecision(9, 6);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Ubicacion).HasColumnType("point");

            entity.HasOne(d => d.Comuna).WithMany(p => p.CentroDeportes)
                .HasForeignKey(d => d.IdComuna)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_centro_deporte_comuna");
        });

        modelBuilder.Entity<Comuna>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comuna");

            entity.HasIndex(e => e.IdRegion, "fk_comuna_region_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Latitud).HasPrecision(9, 6);
            entity.Property(e => e.Longitud).HasPrecision(9, 6);
            entity.Property(e => e.Ubicacion).HasColumnType("point");

            entity.HasOne(d => d.Region).WithMany(p => p.Comunas)
                .HasForeignKey(d => d.IdRegion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comuna_region");
        });

        modelBuilder.Entity<CriterioMvp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("criterio_mvp");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<DetalleFuncionalidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("detalle_funcionalidad");

            entity.HasIndex(e => e.IdFuncionalidad, "fk_detalle_funcionalidad_funcionalidad_idx");

            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.NombreFormulario).HasMaxLength(50);

            entity.HasOne(d => d.Funcionalidad).WithMany(p => p.DetalleFuncionalidades)
                .HasForeignKey(d => d.IdFuncionalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_detalle_funcionalidad_funcionalidad");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("evento");

            entity.HasIndex(e => e.IdCategoriaGeneroPermitido, "fk_evento_categoriagenero_idx");

            entity.HasIndex(e => e.IdCentroDeporte, "fk_evento_centro_deporte_idx");

            entity.HasIndex(e => e.IdMetodoPago, "fk_evento_metodo_pago_idx");

            entity.HasIndex(e => e.IdTipoDeporte, "fk_evento_tipo_deporte_idx");

            entity.HasIndex(e => e.IdTipoDivisa, "fk_evento_tipo_divisa_idx");

            entity.HasIndex(e => e.IdCreador, "fk_evento_usuario_idx");

            entity.Property(e => e.Creador).HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.DireccionCentroDeporte).HasMaxLength(250);
            entity.Property(e => e.FechaCreacion).HasMaxLength(6);
            entity.Property(e => e.FechaEvento).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasMaxLength(6);
            //entity.Property(e => e.HoraEvento).HasColumnType("time");
            entity.Property(e => e.LatitudCentroDeporte).HasPrecision(9, 6);
            entity.Property(e => e.LongitudCentroDeporte).HasPrecision(9, 6);
            entity.Property(e => e.Modificador).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.NombreCentroDeporte).HasMaxLength(50);
            entity.Property(e => e.PrecioPorPersona).HasPrecision(18, 2);

            entity.HasOne(d => d.CategoriaGenero).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdCategoriaGeneroPermitido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evento_categoriagenero");

            entity.HasOne(d => d.CentroDeporte).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdCentroDeporte)
                .HasConstraintName("fk_evento_centro_deporte");

            entity.HasOne(d => d.UsuarioCreador).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evento_usuario");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evento_metodo_pago");

            entity.HasOne(d => d.TipoDeporte).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdTipoDeporte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evento_tipo_deporte");

            entity.HasOne(d => d.TipoDivisa).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdTipoDivisa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_evento_tipo_divisa");
        });

        modelBuilder.Entity<Funcionalidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("funcionalidad");

            entity.Property(e => e.Codigo).HasMaxLength(5);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("metodo_pago");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Nacionalidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("nacionalidad");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notificacion");          

            entity.HasIndex(e => e.IdEvento, "fk_notificacion_evento_idx");

            entity.HasIndex(e => e.IdTipoNotificacion, "fk_notificacion_tipo_notificacion_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_notificacion_usuario_idx");

            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaEnvio).HasColumnType("datetime");
            entity.Property(e => e.Mensaje).HasMaxLength(500);
            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.Evento).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdEvento)                
                .HasConstraintName("fk_notificacion_evento");

            entity.HasOne(d => d.TipoNotificacion).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdTipoNotificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notificacion_tipo_notificacion");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_notificacion_usuario");
        });


        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pais");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("region");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rol");

            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoDeporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipo_deporte");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoDivisa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipo_divisa");

            entity.Property(e => e.Codigo).HasMaxLength(5);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoGenero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipo_genero");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoMembresia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipo_membresia");

            entity.Property(e => e.Descripcion).HasMaxLength(150);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoNotificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipo_notificacion");

            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.Property(e => e.Mensaje).HasMaxLength(500);

            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.IdComuna, "fk_usuario_comuna_idx");

            entity.HasIndex(e => e.IdNacionalidad, "fk_usuario_nacionalidad_idx");

            entity.HasIndex(e => e.IdPaisResidencia, "fk_usuario_pais_idx");

            entity.HasIndex(e => e.IdTipoGenero, "fk_usuario_tipo_genero_idx");

            entity.HasIndex(e => e.IdTipoMembresia, "fk_usuario_tipo_membresia_idx");

            entity.Property(e => e.Latitud).HasPrecision(9, 6);
            entity.Property(e => e.ApellidoMaterno).HasMaxLength(45);
            entity.Property(e => e.ApellidoPaterno).HasMaxLength(50);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.FcmToken).HasMaxLength(250);
            entity.Property(e => e.Foto).HasMaxLength(100);
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Longitud).HasPrecision(9, 6);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RecoveryCode).HasMaxLength(50);
            entity.Property(e => e.RecoveryCodeExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken).HasMaxLength(100);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(100);          

            entity.HasOne(d => d.Comuna).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdComuna)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_comuna");

            entity.HasOne(d => d.Nacionalidad).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdNacionalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_nacionalidad");

            entity.HasOne(d => d.PaisResidencia).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdPaisResidencia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_pais");

            entity.HasOne(d => d.TipoGenero).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_tipo_genero");

            entity.HasOne(d => d.TipoMembresia).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoMembresia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_tipo_membresia");
        });

        modelBuilder.Entity<UsuarioComuna>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario_comuna");

            entity.HasIndex(e => e.IdComuna, "fk_usuario_comuna_comuna_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_usuario_comuna_usuario_idx");

            entity.HasOne(d => d.Comuna).WithMany(p => p.UsuarioComunas)
                .HasForeignKey(d => d.IdComuna)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_comuna_comuna");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioComunas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_comuna_usuario");
        });

        modelBuilder.Entity<UsuarioEvaluacion>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioEvaluacion).HasName("PRIMARY");

            entity.ToTable("usuario_evaluacion");

            entity.HasIndex(e => e.IdCriterioMvp, "fk_usuario_evaluacion_criterio_Mvp_idx");

            entity.HasIndex(e => e.IdEvento, "fk_usuario_evaluacion_evento_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_usuario_evaluacion_usuario_idx");

            entity.HasOne(d => d.CriterioMvp).WithMany(p => p.UsuarioEvaluaciones)
                .HasForeignKey(d => d.IdCriterioMvp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_evaluacion_criterio_Mvp");

            entity.HasOne(d => d.Evento).WithMany(p => p.Evaluaciones)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_evaluacion_evento");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioEvaluaciones)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_evaluacion_usuario");
        });

        modelBuilder.Entity<UsuarioEvento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario_evento");

            entity.HasIndex(e => e.IdEvento, "fk_usuario_evento_evento_idx");

            entity.HasIndex(e => e.IdRol, "fk_usuario_evento_rol_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_usuario_evento_usuario_idx");

            entity.Property(e => e.Posicion).HasMaxLength(100);

            entity.HasOne(d => d.Evento).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_evento_evento");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioEventos)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_evento_rol");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioEventos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_evento_usuario");
        });

        modelBuilder.Entity<UsuarioHorario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario_horario");

            entity.HasIndex(e => e.IdUsuario, "fk_usuario_horario_usuario1_idx");

            entity.Property(e => e.HorarioInicio).HasColumnType("time");
            entity.Property(e => e.HorarioTermino).HasColumnType("time");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioHorarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_horario_usuario");
        });

        modelBuilder.Entity<UsuarioTipoDeporte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario_tipo_deporte");

            entity.HasIndex(e => e.IdTipoDeporte, "fk_usuario_tipo_deporte_tipo_deporte_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_usuario_tipo_deporte_usuario_idx");

            entity.HasOne(d => d.TipoDeporte).WithMany(p => p.UsuarioTipoDeportes)
                .HasForeignKey(d => d.IdTipoDeporte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_tipo_deporte_tipo_deporte");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioTipoDeportes)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuario_tipo_deporte_usuario");
        });

        modelBuilder.Entity<UsuarioTipoNotificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario_tipo_notificacion");

            entity.HasIndex(e => e.IdTipoNotificacion, "fk_usuario_tipo_notificacion_tipo_notificacion_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_usuario_tipo_notificacion_usuario_idx");

            entity.HasOne(d => d.TipoNotificacion).WithMany(p => p.UsuarioTipoNotificaciones)
                .HasForeignKey(d => d.IdTipoNotificacion)
                .HasConstraintName("fk_usuario_tipo_notificacion_tipo_notificacion");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioTipoNotificaciones)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_usuario_tipo_notificacion_usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

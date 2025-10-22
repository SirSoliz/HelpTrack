using System;
using System.Collections.Generic;
using HelpTrack.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpTrack.Infraestructure.Data;

public partial class HelpTrackContext : DbContext
{
    public HelpTrackContext(DbContextOptions<HelpTrackContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AsignacionesTicket> AsignacionesTicket { get; set; }

    public virtual DbSet<Categorias> Categorias { get; set; }

    public virtual DbSet<Especialidades> Especialidades { get; set; }

    public virtual DbSet<EstadosTicket> EstadosTicket { get; set; }

    public virtual DbSet<Etiquetas> Etiquetas { get; set; }

    public virtual DbSet<HistorialTicket> HistorialTicket { get; set; }

    public virtual DbSet<ImagenesTicket> ImagenesTicket { get; set; }

    public virtual DbSet<Notificaciones> Notificaciones { get; set; }

    public virtual DbSet<Prioridades> Prioridades { get; set; }

    public virtual DbSet<ReglasAsignacion> ReglasAsignacion { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Sla> Sla { get; set; }

    public virtual DbSet<Tecnicos> Tecnicos { get; set; }

    public virtual DbSet<Tickets> Tickets { get; set; }

    public virtual DbSet<TiposNotificacion> TiposNotificacion { get; set; }

    public virtual DbSet<UsuarioRoles> UsuarioRoles { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<ValoracionTicket> ValoracionTicket { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AsignacionesTicket>(entity =>
        {
            entity.HasKey(e => e.IdAsignacion).HasName("PK__asignaci__C3F7F9662A9125BD");

            entity.ToTable("asignaciones_ticket");

            entity.HasIndex(e => e.IdTicket, "UQ__asignaci__48C6F522CF0ED919").IsUnique();

            entity.Property(e => e.IdAsignacion).HasColumnName("id_asignacion");
            entity.Property(e => e.FechaAsignacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_asignacion");
            entity.Property(e => e.HorasRestantesSla).HasColumnName("horas_restantes_sla");
            entity.Property(e => e.IdTecnico).HasColumnName("id_tecnico");
            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.Metodo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("metodo");
            entity.Property(e => e.Prioridad).HasColumnName("prioridad");
            entity.Property(e => e.PuntajePrioridad).HasColumnName("puntaje_prioridad");

            entity.HasOne(d => d.IdTecnicoNavigation).WithMany(p => p.AsignacionesTicket)
                .HasForeignKey(d => d.IdTecnico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_asig_tecnico");

            entity.HasOne(d => d.IdTicketNavigation).WithOne(p => p.AsignacionesTicket)
                .HasForeignKey<AsignacionesTicket>(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_asig_ticket");
        });

        modelBuilder.Entity<Categorias>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK__categori__CD54BC5A67994703");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "UQ__categori__72AFBCC662B96382").IsUnique();

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdSla).HasColumnName("id_sla");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdSlaNavigation).WithMany(p => p.Categorias)
                .HasForeignKey(d => d.IdSla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_categorias_sla");

            entity.HasMany(d => d.IdEspecialidad).WithMany(p => p.IdCategoria)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoriaEspecialidad",
                    r => r.HasOne<Especialidades>().WithMany()
                        .HasForeignKey("IdEspecialidad")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_catesp_especialidad"),
                    l => l.HasOne<Categorias>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_catesp_categoria"),
                    j =>
                    {
                        j.HasKey("IdCategoria", "IdEspecialidad").HasName("PK__categori__E149AF2CA9FA0B94");
                        j.ToTable("categoria_especialidad");
                        j.IndexerProperty<short>("IdCategoria").HasColumnName("id_categoria");
                        j.IndexerProperty<short>("IdEspecialidad").HasColumnName("id_especialidad");
                    });

            entity.HasMany(d => d.IdEtiqueta).WithMany(p => p.IdCategoria)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoriaEtiqueta",
                    r => r.HasOne<Etiquetas>().WithMany()
                        .HasForeignKey("IdEtiqueta")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_cateti_etiqueta"),
                    l => l.HasOne<Categorias>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_cateti_categoria"),
                    j =>
                    {
                        j.HasKey("IdCategoria", "IdEtiqueta").HasName("PK__categori__02F4617080DCCEBF");
                        j.ToTable("categoria_etiqueta");
                        j.IndexerProperty<short>("IdCategoria").HasColumnName("id_categoria");
                        j.IndexerProperty<int>("IdEtiqueta").HasColumnName("id_etiqueta");
                    });
        });

        modelBuilder.Entity<Especialidades>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("PK__especial__C1D137635CAB981A");

            entity.ToTable("especialidades");

            entity.HasIndex(e => e.Nombre, "UQ__especial__72AFBCC61B331E93").IsUnique();

            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadosTicket>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__estados___86989FB2E58A1362");

            entity.ToTable("estados_ticket");

            entity.HasIndex(e => e.Nombre, "UQ__estados___72AFBCC619367931").IsUnique();

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.OrdenFlujo).HasColumnName("orden_flujo");
        });

        modelBuilder.Entity<Etiquetas>(entity =>
        {
            entity.HasKey(e => e.IdEtiqueta).HasName("PK__etiqueta__FA0DD2AD454BBEEA");

            entity.ToTable("etiquetas");

            entity.HasIndex(e => e.Nombre, "UQ__etiqueta__72AFBCC611C8CF42").IsUnique();

            entity.Property(e => e.IdEtiqueta).HasColumnName("id_etiqueta");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<HistorialTicket>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__historia__76E6C50275360A8A");

            entity.ToTable("historial_ticket");

            entity.Property(e => e.IdHistorial).HasColumnName("id_historial");
            entity.Property(e => e.FechaEvento)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_evento");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.IdUsuarioAccion).HasColumnName("id_usuario_accion");
            entity.Property(e => e.Observacion).HasColumnName("observacion");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.HistorialTicket)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ht_estado");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.HistorialTicket)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ht_ticket");

            entity.HasOne(d => d.IdUsuarioAccionNavigation).WithMany(p => p.HistorialTicket)
                .HasForeignKey(d => d.IdUsuarioAccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_h_usuario");
        });

        modelBuilder.Entity<ImagenesTicket>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("PK__imagenes__27CC2689943ABD4A");

            entity.ToTable("imagenes_ticket");

            entity.Property(e => e.IdImagen).HasColumnName("id_imagen");
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.NombreArchivo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre_archivo");
            entity.Property(e => e.TipoContenido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo_contenido");
            entity.Property(e => e.UrlArchivo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("url_archivo");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.ImagenesTicket)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__imagenes___id_ti__6754599E");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ImagenesTicket)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__imagenes___id_us__68487DD7");
        });

        modelBuilder.Entity<Notificaciones>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__notifica__8270F9A5137C4506");

            entity.ToTable("notificaciones");

            entity.Property(e => e.IdNotificacion).HasColumnName("id_notificacion");
            entity.Property(e => e.Atendida).HasColumnName("atendida");
            entity.Property(e => e.FechaAtendida)
                .HasPrecision(0)
                .HasColumnName("fecha_atendida");
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdDestinatario).HasColumnName("id_destinatario");
            entity.Property(e => e.IdRemitente).HasColumnName("id_remitente");
            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(500)
                .HasColumnName("mensaje");
            entity.Property(e => e.Titulo)
                .HasMaxLength(120)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdDestinatarioNavigation).WithMany(p => p.NotificacionesIdDestinatarioNavigation)
                .HasForeignKey(d => d.IdDestinatario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_notif_destinatario");

            entity.HasOne(d => d.IdRemitenteNavigation).WithMany(p => p.NotificacionesIdRemitenteNavigation)
                .HasForeignKey(d => d.IdRemitente)
                .HasConstraintName("FK_notif_remitente");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdTicket)
                .HasConstraintName("FK_notif_ticket");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_notif_tipo");
        });

        modelBuilder.Entity<Prioridades>(entity =>
        {
            entity.HasKey(e => e.IdPrioridad).HasName("PK__priorida__EF3DAB401FC88502");

            entity.ToTable("prioridades");

            entity.HasIndex(e => e.Nombre, "UQ__priorida__72AFBCC6870BB203").IsUnique();

            entity.Property(e => e.IdPrioridad).HasColumnName("id_prioridad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.ValorPrioridad).HasColumnName("valor_prioridad");
        });

        modelBuilder.Entity<ReglasAsignacion>(entity =>
        {
            entity.HasKey(e => e.IdRegla).HasName("PK__reglas_a__46D1C1921E24B2E3");

            entity.ToTable("reglas_asignacion");

            entity.Property(e => e.IdRegla).HasColumnName("id_regla");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.ConsiderarCarga)
                .HasDefaultValue(true)
                .HasColumnName("considerar_carga");
            entity.Property(e => e.ConsiderarSla)
                .HasDefaultValue(true)
                .HasColumnName("considerar_sla");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(400)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PesoPrioridad)
                .HasDefaultValue(1000)
                .HasColumnName("peso_prioridad");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__roles__6ABCB5E0F16B19BF");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "UQ__roles__72AFBCC6A5414FC6").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Sla>(entity =>
        {
            entity.HasKey(e => e.IdSla).HasName("PK__sla__6D6C1A3AF9651FB4");

            entity.ToTable("sla");

            entity.Property(e => e.IdSla).HasColumnName("id_sla");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.TiempoResolucionMax).HasColumnName("tiempo_resolucion_max");
            entity.Property(e => e.TiempoRespuestaMax).HasColumnName("tiempo_respuesta_max");
        });

        modelBuilder.Entity<Tecnicos>(entity =>
        {
            entity.HasKey(e => e.IdTecnico).HasName("PK__tecnicos__D55097373E9F475B");

            entity.ToTable("tecnicos");

            entity.Property(e => e.IdTecnico)
                .ValueGeneratedNever()
                .HasColumnName("id_tecnico");
            entity.Property(e => e.Alias)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("alias");
            entity.Property(e => e.Disponible)
                .HasDefaultValue(true)
                .HasColumnName("disponible");
            entity.Property(e => e.NivelCarga).HasColumnName("nivel_carga");

            entity.HasOne(d => d.IdTecnicoNavigation).WithOne(p => p.Tecnicos)
                .HasForeignKey<Tecnicos>(d => d.IdTecnico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tecnicos__id_tec__34C8D9D1");

            entity.HasMany(d => d.IdEspecialidad).WithMany(p => p.IdTecnico)
                .UsingEntity<Dictionary<string, object>>(
                    "TecnicoEspecialidad",
                    r => r.HasOne<Especialidades>().WithMany()
                        .HasForeignKey("IdEspecialidad")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tecnico_e__id_es__3B75D760"),
                    l => l.HasOne<Tecnicos>().WithMany()
                        .HasForeignKey("IdTecnico")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__tecnico_e__id_te__3A81B327"),
                    j =>
                    {
                        j.HasKey("IdTecnico", "IdEspecialidad").HasName("PK__tecnico___F94D8441A0C2BE3A");
                        j.ToTable("tecnico_especialidad");
                        j.IndexerProperty<int>("IdTecnico").HasColumnName("id_tecnico");
                        j.IndexerProperty<short>("IdEspecialidad").HasColumnName("id_especialidad");
                    });
        });

        modelBuilder.Entity<Tickets>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("PK__tickets__48C6F5236F0FEA92");

            entity.ToTable("tickets");

            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaAsignacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_asignacion");
            entity.Property(e => e.FechaCierre)
                .HasPrecision(0)
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdEstadoActual)
                .HasDefaultValue(1)
                .HasColumnName("id_estado_actual");
            entity.Property(e => e.IdEtiqueta).HasColumnName("id_etiqueta");
            entity.Property(e => e.IdPrioridad).HasColumnName("id_prioridad");
            entity.Property(e => e.IdSla).HasColumnName("id_sla");
            entity.Property(e => e.IdUsuarioCreacion).HasColumnName("id_usuario_creacion");
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tickets_categoria");

            entity.HasOne(d => d.IdEstadoActualNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdEstadoActual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tickets_estado");

            entity.HasOne(d => d.IdEtiqueta1).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdEtiqueta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tickets_etiqueta");

            entity.HasOne(d => d.IdPrioridadNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdPrioridad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tickets_prioridad");

            entity.HasOne(d => d.IdSlaNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdSla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tickets_sla");

            entity.HasOne(d => d.IdUsuarioCreacionNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdUsuarioCreacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tickets_cliente");

            entity.HasMany(d => d.IdEtiquetaNavigation).WithMany(p => p.IdTicket)
                .UsingEntity<Dictionary<string, object>>(
                    "TicketEtiquetas",
                    r => r.HasOne<Etiquetas>().WithMany()
                        .HasForeignKey("IdEtiqueta")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ticket_et__id_et__6383C8BA"),
                    l => l.HasOne<Tickets>().WithMany()
                        .HasForeignKey("IdTicket")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ticket_et__id_ti__628FA481"),
                    j =>
                    {
                        j.HasKey("IdTicket", "IdEtiqueta").HasName("PK__ticket_e__8766280909E77B72");
                        j.ToTable("ticket_etiquetas");
                        j.IndexerProperty<long>("IdTicket").HasColumnName("id_ticket");
                        j.IndexerProperty<int>("IdEtiqueta").HasColumnName("id_etiqueta");
                    });
        });

        modelBuilder.Entity<TiposNotificacion>(entity =>
        {
            entity.HasKey(e => e.IdTipo).HasName("PK__tipos_no__CF901089BA7AE480");

            entity.ToTable("tipos_notificacion");

            entity.HasIndex(e => e.Nombre, "UQ__tipos_no__72AFBCC6DC28867C").IsUnique();

            entity.Property(e => e.IdTipo).HasColumnName("id_tipo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<UsuarioRoles>(entity =>
        {
            entity.HasKey(e => new { e.IdUsuario, e.IdRol }).HasName("PK__usuario___5895CFF359A6E2CD");

            entity.ToTable("usuario_roles");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.FechaAsignacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("fecha_asignacion");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario_r__id_ro__2F10007B");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario_r__id_us__2E1BDC42");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuarios__4E3E04AD437208C3");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E61642A9763D7").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(256)
                .HasColumnName("contrasena");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.UltimoInicioSesion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("ultimo_inicio_sesion");
        });

        modelBuilder.Entity<ValoracionTicket>(entity =>
        {
            entity.HasKey(e => e.IdValoracion).HasName("PK__valoraci__1861B2495D029F6E");

            entity.ToTable("valoracion_ticket");

            entity.HasIndex(e => e.IdTicket, "UQ__valoraci__48C6F52252463861").IsUnique();

            entity.Property(e => e.IdValoracion).HasColumnName("id_valoracion");
            entity.Property(e => e.Calificacion).HasColumnName("calificacion");
            entity.Property(e => e.Comentario)
                .HasMaxLength(1000)
                .HasColumnName("comentario");
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdTicket).HasColumnName("id_ticket");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdTicketNavigation).WithOne(p => p.ValoracionTicket)
                .HasForeignKey<ValoracionTicket>(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__valoracio__id_ti__0B91BA14");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ValoracionTicket)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__valoracio__id_us__0C85DE4D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

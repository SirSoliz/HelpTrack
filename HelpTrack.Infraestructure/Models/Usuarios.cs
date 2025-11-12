using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Usuarios
{
    public int IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public byte[]? Contrasena { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? UltimoInicioSesion { get; set; }
    public virtual ICollection<HistorialTicket> HistorialTicket { get; set; } = new List<HistorialTicket>();

    public virtual ICollection<ImagenesTicket> ImagenesTicket { get; set; } = new List<ImagenesTicket>();

    public virtual ICollection<Notificaciones> NotificacionesIdDestinatarioNavigation { get; set; } = new List<Notificaciones>();

    public virtual ICollection<Notificaciones> NotificacionesIdRemitenteNavigation { get; set; } = new List<Notificaciones>();

    public virtual Tecnicos? Tecnicos { get; set; }

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();

    public virtual ICollection<UsuarioRoles> UsuarioRoles { get; set; } = new List<UsuarioRoles>();

    public virtual ICollection<ValoracionTicket> ValoracionTicket { get; set; } = new List<ValoracionTicket>();
}

using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Notificaciones
{
    public long IdNotificacion { get; set; }

    public int IdTipo { get; set; }

    public int? IdRemitente { get; set; }

    public int IdDestinatario { get; set; }

    public long? IdTicket { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Mensaje { get; set; }

    public bool Atendida { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaAtendida { get; set; }

    public virtual Usuarios IdDestinatarioNavigation { get; set; } = null!;

    public virtual Usuarios? IdRemitenteNavigation { get; set; }

    public virtual Tickets? IdTicketNavigation { get; set; }

    public virtual TiposNotificacion IdTipoNavigation { get; set; } = null!;
}

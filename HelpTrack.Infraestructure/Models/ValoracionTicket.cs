using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class ValoracionTicket
{
    public long IdValoracion { get; set; }

    public long IdTicket { get; set; }

    public int IdUsuario { get; set; }

    public byte Calificacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Tickets IdTicketNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}

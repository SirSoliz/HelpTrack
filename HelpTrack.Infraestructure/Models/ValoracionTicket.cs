using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class ValoracionTicket
{
    public int IdValoracion { get; set; }

    public int IdTicket { get; set; }

    public int IdUsuario { get; set; }

    public int Calificacion { get; set; }

    public string? Comentario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Tickets IdTicketNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}

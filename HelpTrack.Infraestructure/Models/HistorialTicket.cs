using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class HistorialTicket
{
    public int IdHistorial { get; set; }

    public int IdTicket { get; set; }

    public int IdEstado { get; set; }

    public int IdUsuarioAccion { get; set; }

    public string? Observacion { get; set; }

    public DateTime FechaEvento { get; set; }

    public virtual EstadosTicket IdEstadoNavigation { get; set; } = null!;

    public virtual Tickets IdTicketNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioAccionNavigation { get; set; } = null!;
}

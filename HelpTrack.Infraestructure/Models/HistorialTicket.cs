using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class HistorialTicket
{
    public long IdHistorial { get; set; }

    public long IdTicket { get; set; }

    public int IdEstado { get; set; }

    public int IdUsuarioAccion { get; set; }

    public string? Observacion { get; set; }

    public DateTime FechaEvento { get; set; }

    public virtual EstadosTicket IdEstadoNavigation { get; set; } = null!;

    public virtual Tickets IdTicketNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioAccionNavigation { get; set; } = null!;
}

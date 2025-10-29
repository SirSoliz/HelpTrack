using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class AsignacionesTicket
{
    public int IdAsignacion { get; set; }

    public int IdTicket { get; set; }

    public int IdTecnico { get; set; }

    public string Metodo { get; set; } = null!;

    public int Prioridad { get; set; }

    public int? HorasRestantesSla { get; set; }

    public int? PuntajePrioridad { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public virtual Tecnicos IdTecnicoNavigation { get; set; } = null!;

    public virtual Tickets IdTicketNavigation { get; set; } = null!;
}

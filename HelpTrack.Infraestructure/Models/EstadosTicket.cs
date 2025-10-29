using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class EstadosTicket
{
    public int IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public int OrdenFlujo { get; set; }

    public virtual ICollection<HistorialTicket> HistorialTicket { get; set; } = new List<HistorialTicket>();

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
}

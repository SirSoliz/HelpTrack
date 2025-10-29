using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Prioridades
{
    public int IdPrioridad { get; set; }

    public string Nombre { get; set; } = null!;

    public int ValorPrioridad { get; set; }

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
}

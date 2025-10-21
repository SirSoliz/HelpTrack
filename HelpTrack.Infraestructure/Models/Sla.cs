using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Sla
{
    public short IdSla { get; set; }

    public string Nombre { get; set; } = null!;

    public int TiempoRespuestaMax { get; set; }

    public int TiempoResolucionMax { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Categorias> Categorias { get; set; } = new List<Categorias>();

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
}

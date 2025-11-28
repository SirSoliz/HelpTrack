using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Categorias
{
    public Categorias()
    {
        IdEtiqueta = new HashSet<Etiquetas>();
        IdEspecialidad = new HashSet<Especialidades>();
    }

    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int IdSla { get; set; }

    public virtual Sla IdSlaNavigation { get; set; } = null!;

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();

    public virtual ICollection<Especialidades> IdEspecialidad { get; set; } = new List<Especialidades>();

    public virtual ICollection<Etiquetas> IdEtiqueta { get; set; } = new List<Etiquetas>();


}

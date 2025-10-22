using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Etiquetas
{
    public int IdEtiqueta { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();

    public virtual ICollection<Categorias> IdCategoria { get; set; } = new List<Categorias>();

    public virtual ICollection<Tickets> IdTicket { get; set; } = new List<Tickets>();
}

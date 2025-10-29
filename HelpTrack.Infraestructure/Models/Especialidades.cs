using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Especialidades
{
    public int IdEspecialidad { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Categorias> IdCategoria { get; set; } = new List<Categorias>();

    public virtual ICollection<Tecnicos> IdTecnico { get; set; } = new List<Tecnicos>();
}

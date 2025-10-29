using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Tecnicos
{
    public int IdTecnico { get; set; }

    public string? Alias { get; set; }

    public bool Disponible { get; set; }

    public int NivelCarga { get; set; }

    public virtual ICollection<AsignacionesTicket> AsignacionesTicket { get; set; } = new List<AsignacionesTicket>();

    public virtual Usuarios IdTecnicoNavigation { get; set; } = null!;

    public virtual ICollection<Especialidades> IdEspecialidad { get; set; } = new List<Especialidades>();
}

using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class ReglasAsignacion
{
    public int IdRegla { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int PesoPrioridad { get; set; }

    public bool ConsiderarSla { get; set; }

    public bool ConsiderarCarga { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }
}

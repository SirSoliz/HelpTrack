using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class Tickets
{
    public int IdTicket { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaAsignacion { get; set; }

    public DateTime? FechaCierre { get; set; }

    public int IdUsuarioCreacion { get; set; }

    public int IdCategoria { get; set; }

    public int IdPrioridad { get; set; }

    public int IdEstadoActual { get; set; }

    public int IdSla { get; set; }

    public int IdEtiqueta { get; set; }

    public virtual AsignacionesTicket? AsignacionesTicket { get; set; }

    public virtual ICollection<HistorialTicket> HistorialTicket { get; set; } = new List<HistorialTicket>();

    public virtual Categorias IdCategoriaNavigation { get; set; } = null!;

    public virtual EstadosTicket IdEstadoActualNavigation { get; set; } = null!;

    public virtual Etiquetas IdEtiqueta1 { get; set; } = null!;

    public virtual Prioridades IdPrioridadNavigation { get; set; } = null!;

    public virtual Sla IdSlaNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioCreacionNavigation { get; set; } = null!;

    public virtual ICollection<ImagenesTicket> ImagenesTicket { get; set; } = new List<ImagenesTicket>();

    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();

    public virtual ValoracionTicket? ValoracionTicket { get; set; }

    public virtual ICollection<Etiquetas> IdEtiquetaNavigation { get; set; } = new List<Etiquetas>();
}

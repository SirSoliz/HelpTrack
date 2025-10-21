using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class TiposNotificacion
{
    public int IdTipo { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();
}

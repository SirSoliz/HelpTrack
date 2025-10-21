using System;
using System.Collections.Generic;

namespace HelpTrack.Infraestructure.Models;

public partial class ImagenesTicket
{
    public long IdImagen { get; set; }

    public long IdTicket { get; set; }

    public int IdUsuario { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string TipoContenido { get; set; } = null!;

    public string UrlArchivo { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual Tickets IdTicketNavigation { get; set; } = null!;

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}

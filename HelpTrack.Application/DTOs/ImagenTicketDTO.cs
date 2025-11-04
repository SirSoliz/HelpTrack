using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public record ImagenTicketDTO
    {
        public long IdImagen { get; set; }

        public int IdTicket { get; set; }

        public int IdUsuario { get; set; }

        public string NombreArchivo { get; set; } = null!;

        public string TipoContenido { get; set; } = null!;

        public string UrlArchivo { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public virtual Tickets IdTicketNavigation { get; set; } = null!;

        public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
    }
}

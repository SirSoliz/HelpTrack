using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public class HistorialTicketDTO
    {
        public int IdHistorial { get; set; }
        public int IdTicket { get; set; }
        public string? Estado { get; set; }
        public string? UsuarioAccion { get; set; }
        public string? Observacion { get; set; }
        public DateTime FechaEvento { get; set; }
    }
}

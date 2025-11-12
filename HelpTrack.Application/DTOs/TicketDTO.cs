using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public record TicketDTO
    {
        public long IdTicket { get; set; }

        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public int IdPrioridad { get; set; }

        public int IdEstadoActual { get; set; }

        public int IdUsuarioCreacion { get; set; } 

        public EstadoTicketDTO? EstadoActual { get; set; }

        public virtual EstadosTicket IdEstadoActualNavigation { get; set; } = null!;

        public virtual ICollection<ImagenesTicket> ImagenesTicket { get; set; } = new List<ImagenesTicket>();

        public virtual List<CategoriaDTO> IdCategoria { get; set; } = null!;

        public virtual TecnicoDTO IdTecnico { get; set; } = null!;

        public virtual ICollection<EtiquetaDTO> IdEtiquetaNavigation { get; set; }
    }
}
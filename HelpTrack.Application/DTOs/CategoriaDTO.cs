using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public record CategoriaDTO
    {
        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = null!;

        //public virtual List<LibroDTO> IdLibro { get; set; } = null!;
    }
}

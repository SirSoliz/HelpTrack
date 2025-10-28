using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public class EtiquetaDTO
    {
        public short IdEtiqueta { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}

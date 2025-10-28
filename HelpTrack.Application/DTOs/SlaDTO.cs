using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public class SlaDTO
    {
        public short IdSla { get; set; }
        public string Nombre { get; set; }
        public int TiempoMaxRespuestaHoras { get; set; }
        public int TiempoMaxResolucionHoras { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class rptUtilizacionDTO
    {
        public string Nombre { get; set; }
        public decimal hrsProyectadas { get; set; }
        public decimal hrsRegistradasTrabajo { get; set; }
        public decimal hrsFaltantesRegistro { get; set; }
        public decimal porRegistro { get; set; }
        public decimal porHorasEfectivas { get; set; }
    }
}

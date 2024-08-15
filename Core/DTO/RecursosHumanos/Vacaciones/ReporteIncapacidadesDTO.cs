using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class ReporteIncapacidadesDTO
    {
        public string concepto { get; set; }
        public int total { get; set; }
        public List<ReporteIncapacidadesDetDTO> lstIncapsDet { get; set; }
    }
}

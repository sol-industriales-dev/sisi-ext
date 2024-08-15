using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas
{
   public class ReporteEjecutivoDTO
    {
       public string cc { get; set; }
       public string numeroContrato { get; set; }
       public string subContratista { get; set; }
       public decimal confiabilidad { get; set; }
       public decimal calificacionEval { get; set; }
       public decimal cumplimientoSoporte{ get; set; }
       public decimal cumplimientoCompromisos{ get; set; }
    }
}

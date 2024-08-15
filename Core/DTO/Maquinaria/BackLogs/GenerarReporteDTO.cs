using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class GenerarReporteDTO
    {
        public List<string> lstTablaDatosBLActuales { get; set; }
        public List<string> lstTablaDatosBLPorc { get; set; }
        public List<string> lstTablaDatosBLTiempoPromedio { get; set; }
        public List<string> lstTendenciaBLRegistrados { get; set; }
        public List<string> lstTendenciaBLCerrados { get; set; }
        public List<string> lstTendenciaBLAcumulados { get; set; }
        public string grafica { get; set; }
    }
}
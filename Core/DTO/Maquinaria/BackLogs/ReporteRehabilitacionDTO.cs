using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class ReporteRehabilitacionDTO
    {
        public string numeroEconomico { get; set; }
        public string descripcion { get; set; }
        public string sistemaAReparar { get; set; }
        public int porcentajeAvance { get; set; }
        public decimal costoRefaccionDLL { get; set; }
        public decimal costoRefaccionMN { get; set; }
        public decimal costoManoObra { get; set; }
        public decimal granTotal { get; set; }
        public int totalBL { get; set; }
        public int totalInstalados { get; set; }
    }
}

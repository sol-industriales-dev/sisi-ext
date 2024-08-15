using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ReportesRH
{
    public class ReporteDashboardTiposCambiosDTO
    {
        public string CamposCambiados { get; set; }
        public int cantidad { get; set; }
        public decimal? porcCambios { get; set; }
    }
}

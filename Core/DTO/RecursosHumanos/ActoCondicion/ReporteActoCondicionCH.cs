using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class ReporteActoCondicionCH
    {
        public string fechaReporte { get; set; }
        public string nombreTrabajador { get; set; }
        public string parrafoUno { get; set; }
        public string razonSocialEmpresa { get; set; }
        public string nombreQuienAplica { get; set; }
        public int idTipoReporte { get; set; }
        public int idActo { get; set; }
    }
}

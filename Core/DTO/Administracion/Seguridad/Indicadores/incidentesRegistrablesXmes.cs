using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class incidentesRegistrablesXmes
    {
        public int mesID { get; set; }
        public string mes { get; set; }
        public string cc { get; set; }
        public decimal cantidad { get; set; }
        public decimal lostDay { get; set; }
        public decimal tasaIncidencia { get; set; }
        public DateTime mesAño { get; set; }
        public int idAgrupacion { get; set; }
        public int idEmpresa { get; set; }
    }
}

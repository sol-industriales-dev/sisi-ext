using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class EmpleadoResguardoDTO
    {
        public int empleado { get; set; }
        public string descripcion { get; set; }
        public int puesto { get; set; }
        public string puestoDesc { get; set; }
        public string telefono { get; set; }
        public decimal? monto { get; set; }
        public decimal? monto_inicial { get; set; }
        public decimal? vobo_monto_inicial { get; set; }
        public decimal? vobo_monto_final { get; set; }
        public string vobo { get; set; }

        public string nom_empleado { get; set; }
        public string ap_paterno_empleado { get; set; }
        public string ap_materno_empleado { get; set; }
        public string rfc_empleado { get; set; }
    }
}

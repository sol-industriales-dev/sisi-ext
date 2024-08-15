using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.RelacionPoliza
{
    public class PolizaRelacionadaDTO
    {
        public int id { get; set; }
        public int tm { get; set; }
        public string factura { get; set; }
        public string concepto { get; set; }
        public string poliza { get; set; }
        public string cuenta { get; set; }
        public decimal monto { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public DateTime? fechaInicioDep { get; set; }
        public decimal? porcentajeDep { get; set; }
        public int? mesesDep { get; set; }
        public string polizaRelacion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.Casos_especiales
{
    public class tblC_AF_DepreciacionEspecial
    {
        public int id { get; set; }
        public decimal monto { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal? porcentajeDepreciacion { get; set; }
        public int? mesesDepreciacion { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime? fechaInicioDepreciacion { get; set; }
        public DateTime? fechaFina { get; set; }
        public decimal? montoDepreciacion { get; set; }
        public int ctaDepreciacion { get; set; }
        public int sctaDepreciacion { get; set; }
        public int ssctaDepreciacion { get; set; }
        public int ctaSaldo { get; set; }
        public int sctaSaldo { get; set; }
        public int ssctaSaldo { get; set; }
        public bool estatus { get; set; }
    }
}

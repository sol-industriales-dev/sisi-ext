using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_ExcelNoMaquinasFaltantes
    {
        public int id { get; set; }
        public int numero { get; set; }
        public int cuenta { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public string descripcion { get; set; }
        public decimal? monto { get; set; }
        public string motivo { get; set; }
    }
}

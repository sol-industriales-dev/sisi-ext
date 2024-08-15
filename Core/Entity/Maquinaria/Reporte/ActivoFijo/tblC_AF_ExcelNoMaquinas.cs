using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_ExcelNoMaquinas
    {
        public int id { get; set; }
        public int numero { get; set; }
        public int cuenta { get; set; }
        public int subcuenta { get; set; }
        public int subsubcuenta { get; set; }
        public DateTime fechaAlta { get; set; }
        public DateTime fechaInicioDep { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public decimal moi { get; set; }
        public decimal altas { get; set; }
        public decimal componentes { get; set; }
        public DateTime? fechaBaja { get; set; }
        public int? polizaBaja { get; set; }
        public string tpBaja { get; set; }
        public decimal porcentaje { get; set; }
        public int mesesDep { get; set; }
    }
}

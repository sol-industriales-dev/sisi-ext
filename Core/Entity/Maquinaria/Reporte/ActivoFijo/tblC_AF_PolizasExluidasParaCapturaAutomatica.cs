using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_PolizasExcluidasParaCapturaAutomatica
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Poliza { get; set; }
        public string TipoPoliza { get; set; }
        public int Linea { get; set; }
        public bool Estatus { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion
{
    public class tblC_AF_EstatusPoliza
    {
        public int Id { get; set; }
        public char Estatus { get; set; }
        public string Descripcion { get; set; }
    }
}
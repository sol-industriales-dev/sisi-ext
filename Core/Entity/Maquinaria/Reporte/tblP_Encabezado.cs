using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte
{
    public class tblP_Encabezado
    {
        public int id { get; set; }
        public string logo { get; set; }
        public string titulo { get; set; }
        public string nombreEmpresa { get; set; }
        public string nombreReporte { get; set; }
        public string area { get; set; }
    }
}

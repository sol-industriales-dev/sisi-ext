using Core.Enum.Maquinaria.Reportes.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.ActivoFijo
{
    public class tblC_AF_PolizaEspecial
    {
        public int id { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public int poliza { get; set; }
        public string tp { get; set; }
        public int linea { get; set; }
        public ComportamientoPolizaEnum comportamientoId { get; set; }
        public bool registroActivo { get; set; }
    }
}

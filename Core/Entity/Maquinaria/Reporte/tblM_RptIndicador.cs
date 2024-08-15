using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte
{
    public class tblM_RptIndicador
    {
        public int id { get; set; }
        public int Tipo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string datosJson { get; set; }
        public string Conclusion { get; set; }
        public decimal Tc { get; set; }
        public string CC { get; set; }
    }
}

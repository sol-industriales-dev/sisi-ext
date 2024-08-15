using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Desempeno
{
    public class ReporteDesempenoDTO
    {
        public int procesoID { get; set; }
        public string proceso { get; set; }
        public int periodoID { get; set; }
        public string periodo { get; set; }
        public int empleadoID { get; set; }
        public string empleado { get; set; }
        public int evaluadorID { get; set; }
        public string evaluador { get; set; }
        public int puestoID { get; set; }
        public string puesto { get; set; }
        public decimal porcentaje { get; set; }
        public int estatus { get; set; }
        public string strEstatus { get; set; }
    }
}

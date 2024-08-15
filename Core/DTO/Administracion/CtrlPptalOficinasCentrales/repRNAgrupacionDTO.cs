using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class repRNAgrupacionDTO
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int idCC { get; set; }
        public string agrupacion { get; set; }
        public string descripcion { get; set; }
        public decimal total { get; set; }
        public bool esMatch { get; set; }
    }
}

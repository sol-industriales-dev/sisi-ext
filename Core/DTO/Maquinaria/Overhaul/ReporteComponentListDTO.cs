using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ReporteComponentListDTO
    {
        public string noComponente { get; set; }
        public string subconjunto { get; set; }
        public string fecha { get; set; }
        public string fechaProxRemocion { get; set; }
        public decimal horaCicloActual { get; set; }
        public decimal target { get; set; }
        public decimal horasAcumuladas { get; set; }
        public string locacion { get; set; }
        public string descripcion { get; set; }
        public int vidas { get; set; }
        public int estatus { get; set; }
        public string diasAlmacenado { get; set; }
    }
}

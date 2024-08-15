using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores
{
    public class PreguntasDTO
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public string pregunta { get; set; }
        public string estatus { get; set; }
        public int tipo { get; set; }
        public string descripcionTipo { get; set; }
        public bool visible { get; set; }
        public int orden { get; set; }
        public decimal ponderacion { get; set; }
    }
}

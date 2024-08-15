using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_PP_PlantillaPersonal_AutDTO
    {
        public string id { get; set; }
        public string plantillaID { get; set; }
        public string aprobadorClave { get; set; }
        public string aprobadorNombre { get; set; }
        public string aprobadorPuesto { get; set; }
        public string tipo { get; set; }
        public string estatus { get; set; }
        public string firma { get; set; }
        public string autorizando { get; set; }
        public string orden { get; set; }
        public string comentario { get; set; }
        public string fecha { get; set; }
    }
}

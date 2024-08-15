using Core.Enum.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MR_TipoDeRespuestas
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int tipoRespuesta { get; set; }
        public string respuestaDesc { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.MatrizDeRiesgo
{
    public class tblCO_MR_TiposDeRepuestaRiesgo
    {
        public int id { get; set; }
        public int idMatrizDeRiesgo { get; set; }
        public int tipoDeRepuesta { get; set; }
        public string descripcion { get; set; }
        public string comentario { get; set; }
        public bool esActivo { get; set; }
        public int idUsuario { get; set; }

    }
}

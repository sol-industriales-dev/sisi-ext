using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_EncuestaProveedores
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public int creadorID { get; set; }
        public virtual tblP_Usuario creador { get; set; }
        public DateTime fecha { get; set; }
        public bool estatus { get; set; }
        public virtual List<tblEN_PreguntasProveedores> preguntas { get; set; }
        public int tipoEncuesta { get; set; }

    }
}

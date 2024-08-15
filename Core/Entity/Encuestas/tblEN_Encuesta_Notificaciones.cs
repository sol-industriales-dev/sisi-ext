using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_Encuesta_Notificaciones
    {

        public int id { get; set; }
        public int encuestaID { get; set; }
        public int usuarioID { get; set; }
        public bool estatus { get; set; }
    }

}

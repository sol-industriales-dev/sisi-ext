using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_Encuesta_Update
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public string objData { get; set; }
        public int usuarioID { get; set; }
        public DateTime fecha { get; set; }
        public int estatus { get; set; }
    }
}

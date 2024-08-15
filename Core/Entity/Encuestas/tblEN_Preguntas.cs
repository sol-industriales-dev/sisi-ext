using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Encuestas
{
    public class tblEN_Preguntas
    {
        public int id { get; set; }
        public int encuestaID { get; set; }
        public virtual tblEN_Encuesta encuesta { get; set; }
        public string pregunta { get; set; }
        public string estatus { get; set; }
        public int tipo { get; set; }

        public bool visible { get; set; }
    }
}

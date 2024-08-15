using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Cursos
{
    public class tblCU_ExamenRespuesta
    {
        public int id { get; set; }
        public int idPregunta { get; set; }
        public string opcion { get; set; }
        public string respuesta { get; set; }
        public bool correcta { get; set; }
    }
}

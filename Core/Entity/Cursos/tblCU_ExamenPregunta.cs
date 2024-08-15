using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Cursos
{
    public class tblCU_ExamenPregunta
    {
        public int  id { get; set; }
        public int idExamen { get; set; }
        public string  pregunta { get; set; }
        public bool  abierta { get; set; }
    }
}

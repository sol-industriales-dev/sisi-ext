using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_ComentarioActividadOverhaul
    {
        public int id { get; set; }
        public string comentario { get; set; }
        public DateTime fecha { get; set; }
        public int eventoID { get; set; }
        public int actividadID { get; set; }
        public int tipo { get; set; }
        public string nombreArchivo { get; set; }
        public int numDia { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_Mensaje
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int mes { get; set; }
        public int idCC { get; set; }
        public string mensaje { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public bool registroActivo { get; set; }
    }
}

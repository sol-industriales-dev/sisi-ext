using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SeguimientoAcuerdos
{
    public class tblSA_Responsables
    {
        public int id { get; set; }
        public int minutaID { get; set; }
        public int actividadID { get; set; }
        public virtual tblSA_Actividades actividad { get; set; }
        public int usuarioID { get; set; }
        public string usuarioText { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public virtual tblSA_Minuta minuta { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SeguimientoAcuerdos
{
    public class tblSA_Interesados
    {
        public int id { get; set; }
        public int minutaID { get; set; }
        public int actividadID { get; set; }
        public tblSA_Actividades actividad { get; set; }
        public int interesadoID { get; set; }
        public string interesado { get; set; }
        public virtual tblSA_Minuta minuta { get; set; }
    }
}

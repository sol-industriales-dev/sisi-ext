using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SeguimientoAcuerdos
{
    public class tblSA_Participante
    {
        public int id { get; set; }
        public int minutaID { get; set; }
        public int participanteID { get; set; }
        public string participante { get; set; }
        public virtual tblSA_Minuta minuta { get; set; }
    }
}

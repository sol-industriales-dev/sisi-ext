using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class tblSA_InteresadosDTO
    {
        public int id { get; set; }
        public int minutaID { get; set; }
        public int actividadID { get; set; }
        public int interesadoID { get; set; }
        public string interesado { get; set; }
    }
}

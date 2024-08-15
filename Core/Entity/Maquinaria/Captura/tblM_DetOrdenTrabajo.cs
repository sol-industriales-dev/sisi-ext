using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_DetOrdenTrabajo
    {
        public int id { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int PersonalID { get; set; }
        public decimal HorasTrabajo { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public int Tipo { get; set; }
        public virtual tblM_CapOrdenTrabajo OrdenTrabajo { get; set; }

    }

}

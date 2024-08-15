using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.MAZDA
{
    public class tblMAZ_Usuario_Actividad
    {
        public int id { get; set; }
        public int usuarioCuadrillaID { get; set; }
        public int actividadPeriodoID { get; set; }
    }
}

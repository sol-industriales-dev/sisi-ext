using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Evaluacion
{
    public class tblSED_Actividad
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal ponderacion { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Requerimientos
{
    public class tblNOM_Evaluacion
    {
        public int id { get; set; }
        public int evidenciaID { get; set; }
        public int indicadorID { get; set; }
        public bool aprobado { get; set; }
        public decimal calificacion { get; set; }
        public bool estatus { get; set; }
    }
}

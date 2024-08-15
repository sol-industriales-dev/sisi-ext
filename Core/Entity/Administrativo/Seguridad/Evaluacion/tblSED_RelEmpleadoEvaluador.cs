using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Evaluacion
{
    public class tblSED_RelEmpleadoEvaluador
    {
        public int id { get; set; }
        public int empleadoID { get; set; }
        public int evaluadorID { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}

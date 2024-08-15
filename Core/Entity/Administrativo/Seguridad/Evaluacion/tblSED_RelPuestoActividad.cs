using Core.Enum.Administracion.Seguridad.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Evaluacion
{
    public class tblSED_RelPuestoActividad
    {
        public int id { get; set; }
        public int puestoID { get; set; }
        public int actividadID { get; set; }
        public PeriodicidadEnum periodicidad { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }
    }
}

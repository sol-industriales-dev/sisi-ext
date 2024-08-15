using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Indicadores;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesInformacionColaboradoresDetalle
    {
        public int id { get; set; }
        public string cveEmpleado { get; set; }
        public int lostDayEmpleado { get; set; }
        public string nombre { get; set; }
        public ClasificacionHHTEnum clasificacion { get; set; }
        public bool estatus { get; set; }
        public int idIncidente { get; set; }
        public virtual tblS_IncidentesInformacionColaboradores Incidente { get; set; }
    }
}

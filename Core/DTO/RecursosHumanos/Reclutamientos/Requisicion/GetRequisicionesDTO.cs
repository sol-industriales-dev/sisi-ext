using Core.DTO.Enkontrol.Tablas.RH.Empleado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos.Requisicion
{
    public class GetRequisicionesDTO : sn_requisicion_personal
    {
        public string puestoDescripcion { get; set; }
        public string categoriaDescripcion { get; set; }
        public string ccDescripcion { get; set; }
        public string nombreJefeInmediato { get; set; }
        public string nombreSolicita { get; set; }
        public string nombreAutoriza { get; set; }
        public bool esAutorizante { get; set; }
        public bool puedeEliminar { get; set; }
        public int idSigoplan { get; set; }
    }
}

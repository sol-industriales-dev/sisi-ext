using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Incapacidades
    {
        public int id { get; set; }
        public int estatus { get; set; }
        public string cc { get; set; }
        public int clave_empleado { get; set; }
        public string codigoIncap { get; set; }
        public int tipoIncap { get; set; }
        public int tipoIncap2 { get; set; }
        public DateTime fechaInicio { get; set; }
        public int totalDias { get; set; }
        public DateTime fechaTerminacion { get; set; }
        public string motivoIncap { get; set; }
        public bool esNotificada { get; set; }
        public bool esAplicadaIncidencias { get; set; }
        public int? idIncidencia { get; set; }
        public DateTime? fechaAplicadas { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

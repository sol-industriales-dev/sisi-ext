using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Vacaciones
{
    public class IncapacidadesDTO
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
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        //RPT
        public string descIncap { get; set; }
        public string descIncap2 { get; set; }
        public string descEstatus { get; set; }
        //JOINS
        public string nombreCompleto { get; set; }
        public string puestoDesc { get; set; }
        public string nss { get; set; }
        public int clave_reg_pat { get; set; }
        public string nombre_corto { get; set; }
        public string ccDescripcion { get; set; }
        public string ccDesc { get; set; }//clave+desc
        public string evidenciaIncap { get; set; }
        public string nombreUsuarioCapturo { get; set; }

        //FRONT
        public bool esVencida { get; set; }
        public int? diasVencer { get; set; }

        public int CantEmpleadosIncapacidades { get; set; }
    }
}

using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionControlAsistenciaDetalle
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string puesto { get; set; }
        public string cc { get; set; }
        public int examenID { get; set; }
        public EstatusEmpledoControlAsistenciaEnum estatus { get; set; }
        public decimal calificacion { get; set; }
        public EstatusAutorizacionEmpleadoControlAsistenciaEnum estatusAutorizacion { get; set; }
        public string rutaExamenInicial { get; set; }
        public string rutaExamenFinal { get; set; }
        public string rutaDC3 { get; set; }
        public int controlAsistenciaID { get; set; }
        public virtual tblS_CapacitacionControlAsistencia controlAsistencia { get; set; }
        public int division { get; set; }
        public bool migrado { get; set; }
    }
}

using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad
{
    public class tblS_CapacitacionSeguridadControlAsistenciaDetalle
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string puesto { get; set; }
        public string cc { get; set; }
        public int examenID { get; set; }
        public int estatus { get; set; }
        public decimal calificacion { get; set; }
        public int estatusAutorizacion { get; set; }
        public string rutaExamenInicial { get; set; }
        public string rutaExamenFinal { get; set; }
        public string rutaDC3 { get; set; }
        public int controlAsistenciaID { get; set; }
        public virtual tblS_CapacitacionSeguridadControlAsistencia controlAsistencia { get; set; }
        public int division { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum EstatusAutorizacionEmpleadoControlAsistenciaEnum
    {
        [DescriptionAttribute("No aplica")]
        NoAplica = 1,
        [DescriptionAttribute("Pendiente de autorización")]
        PendienteAutorizacion = 2,
        [DescriptionAttribute("Autorizado")]
        Autorizado = 3,
        [DescriptionAttribute("Rechazado")]
        Rechazado = 4
    }
}

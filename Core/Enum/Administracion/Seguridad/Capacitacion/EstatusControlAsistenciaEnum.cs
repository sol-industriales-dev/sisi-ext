using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum EstatusControlAsistenciaEnum
    {
        [DescriptionAttribute("Pendiente de cargar lista asistencia")]
        PendienteCargaAsistencia = 1,
        [DescriptionAttribute("Pendiente de gestionar evaluación")]
        PendienteGestionarEvaluacion = 2,
        [DescriptionAttribute("Pendiente de autorización electrónica")]
        PendienteAutorizacion = 3,
        [DescriptionAttribute("Pendiente de cargar DC-3")]
        PendienteCargaDC3 = 5,
        [DescriptionAttribute("Completa")]
        Completa = 4
    }
}

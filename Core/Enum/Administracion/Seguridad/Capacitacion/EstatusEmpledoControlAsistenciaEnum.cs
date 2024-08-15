using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum EstatusEmpledoControlAsistenciaEnum
    {
        [DescriptionAttribute("Pendiente de evaluación")]
        PendienteEvaluacion = 1,
        [DescriptionAttribute("Aprobado")]
        Aprobado = 2,
        [DescriptionAttribute("Reprobado")]
        Reprobado = 3
    }
}

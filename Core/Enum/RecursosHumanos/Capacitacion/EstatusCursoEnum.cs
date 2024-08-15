using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Capacitacion
{
    public enum EstatusCursoEnum
    {
        [DescriptionAttribute("Pendiente de cargar exámenes")]
        PendienteExamenes = 1,
        [DescriptionAttribute("Completo")]
        Completo = 2
    }
}

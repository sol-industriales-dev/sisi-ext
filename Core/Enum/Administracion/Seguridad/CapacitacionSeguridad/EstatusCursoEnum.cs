using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Enum.Administracion.Seguridad.CapacitacionSeguridad
{
    public enum EstatusCursoEnum
    {
        [DescriptionAttribute("Pendiente de cargar exámenes")]
        PendienteExamenes = 1,
        [DescriptionAttribute("Completo")]
        Completo = 2
    }
}

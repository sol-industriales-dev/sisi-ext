using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.SaludOcupacional
{
    public enum TipoAtencionMedicaEnum
    {
        [DescriptionAttribute("ENFERMEDAD")]
        enfermedad = 1,
        [DescriptionAttribute("RIESGO TRABAJO")]
        riesgoTrabajo = 2
    }
}

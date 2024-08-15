using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.FileManager
{
    public enum tipoDocEnvioEnum
    {
        [DescriptionAttribute("Plantilla Personal")]
        PLANTILLA_PERSONAL = 16,
        [DescriptionAttribute("Solicitud de Equipo")]
        SOLICITUD_EQUIPO = 17,
        [DescriptionAttribute("Minuta y lista de asistencia")]
        MINUTA = 18,
        [DescriptionAttribute("Cuadro de Facultamiento")]
        CUADRO_FACULTAMIENTO = 19
    }
}

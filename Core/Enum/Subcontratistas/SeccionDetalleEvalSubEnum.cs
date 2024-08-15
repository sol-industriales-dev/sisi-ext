using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Subcontratistas
{
    public enum SeccionDetalleEvalSubEnum
    {
        [DescriptionAttribute("Cumplimiento de Calidad")]
        MANDO_CUMPLIMIENTO_CALIDAD = 1,
        [DescriptionAttribute("Cumplimiento por planeación")]
        MANDO_CUMPLIMIENTO_PLANEACION = 2,
        [DescriptionAttribute("Cumplimiento por facturación")]
        MANDO_CUMPLIMIENTO_FACTURACION = 3,
        [DescriptionAttribute("Cumplimiento por seguridad")]
        MANDO_CUMPLIMIENTO_SEGURIDAD = 4,
        [DescriptionAttribute("Cumplimiento Ambiental")]
        MANDO_CUMPLIMIENTO_AMBIENTAL = 5,
        [DescriptionAttribute("Cumplimiento por efectividad")]
        MANDO_CUMPLIMIENTO_EFECTIVIDAD = 6,
        [DescriptionAttribute("Cumplimiento por fuerza")]
        MANDO_CUMPLIMIENTO_FUERZA = 7,


    }
}

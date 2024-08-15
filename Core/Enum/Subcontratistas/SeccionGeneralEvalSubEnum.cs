using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Subcontratistas
{
    public enum SeccionGeneralEvalSubEnum
    {
        [DescriptionAttribute("Cumplimiento global por subcontratista")]
        MANDO_CUMPLIMIENTO_GLOBAL_SUBCONTRATISTA = 1,
        [DescriptionAttribute("Cumplimiento global por elementos")]
        MANDO_CUMPLIMIENTO_GLOBAL_ELEMENTOS = 2,
        [DescriptionAttribute("Cumplimiento global por evaluación")]
        MANDO_CUMPLIMIENTO_GLOBAL_EVALUACION = 3,

    }
}

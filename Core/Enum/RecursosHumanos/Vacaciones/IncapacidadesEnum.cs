using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Vacaciones
{
    public enum IncapacidadesEnum
    {
        [DescriptionAttribute("Probable riesgo de trabajo")]
        ProbableRiesgoTrabajo = 0,
        [DescriptionAttribute("Riesgo de trabajo")]
        RiesgoTrabajo = 1,
        [DescriptionAttribute("Enfermedad general")]
        EnfermedadGeneral = 2,
        [DescriptionAttribute("Maternidad")]
        Maternidad = 3,
        [DescriptionAttribute("ST7")]
        ST7 = 4,
        [DescriptionAttribute("ST4")]
        ST4 = 5,
        [DescriptionAttribute("ST2")]
        ST2 = 6,
        [DescriptionAttribute("ST2 Calificada")]
        ST2Calificada = 7,
    }
}

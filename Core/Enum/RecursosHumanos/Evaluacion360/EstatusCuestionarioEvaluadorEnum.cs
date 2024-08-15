using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Evaluacion360
{
    public enum EstatusCuestionarioEvaluadorEnum
    {
        [DescriptionAttribute("No iniciada")]
        NO_INICIADA = 0,
        [DescriptionAttribute("En proceso")]
        EN_PROCESO = 1,
        [DescriptionAttribute("Contestada")]
        CONTESTADA = 2
    }
}

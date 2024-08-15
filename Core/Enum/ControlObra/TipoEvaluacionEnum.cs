using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum TipoEvaluacionEnum
    {
        [DescriptionAttribute("INICIAL")]
        INICIAL = 1,
        [DescriptionAttribute("PERIÓDICA")]
        PERIODICA = 2,
        [DescriptionAttribute("FINAL")]
        FINAL = 3
    }
}

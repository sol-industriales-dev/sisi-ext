using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum TipoEvidenciaEnum
    {
        [DescriptionAttribute("INICIAL")]
        INICIAL = 1,
        [DescriptionAttribute("COMPROMISO")]
        COMPROMISO = 2
    }
}

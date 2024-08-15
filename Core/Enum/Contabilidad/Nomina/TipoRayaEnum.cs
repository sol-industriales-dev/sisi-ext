using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Contabilidad.Nomina
{
    public enum TipoRayaEnum
    {
        [DescriptionAttribute("Nómina")]
        NOMINA = 1,
        [DescriptionAttribute("Finiquito")]
        FINIQUITO = 2,
        [DescriptionAttribute("Aguinaldo")]
        AGUINALDO = 3,
        [DescriptionAttribute("PTU/Bono")]
        PTU_BONO = 4
    }
}

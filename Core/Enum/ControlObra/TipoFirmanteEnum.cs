using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum TipoFirmanteEnum
    {
        [DescriptionAttribute("SUBCONTRATISTA")]
        SUBCONTRATISTA = 1,
        [DescriptionAttribute("GERENTE")]
        GERENTE = 2
    }
}

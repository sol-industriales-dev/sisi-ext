using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum TipoRespuestaEnum
    {
        [DescriptionAttribute("AMENAZA")]
        amenaza = 1,
        [DescriptionAttribute("OPORTUNIDAD")]
        oportunidad = 2
    }
}

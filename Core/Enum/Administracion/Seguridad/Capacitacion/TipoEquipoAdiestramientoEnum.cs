using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum TipoEquipoAdiestramientoEnum
    {
        [DescriptionAttribute("MAYOR")]
        MAYOR = 1,
        [DescriptionAttribute("AUXILIAR")]
        AUXILIAR = 2
    }
}

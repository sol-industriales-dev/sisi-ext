using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion.CincoS
{
    public enum TipoArchivoEnum
    {
        [DescriptionAttribute("DETECCIÓN")]
        DETECCION = 0,
        [DescriptionAttribute("MEDIDA")]
        MEDIDA = 1,
        [DescriptionAttribute("SEGUIMIENTO")]
        SEGUIMIENTO = 2
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum TipoAdiestramientoEnum
    {
        [DescriptionAttribute("NO ASIGNADO")]
        NO_ASIGNADO = 0,
        [DescriptionAttribute("HORAS")]
        HORAS = 1,
        [DescriptionAttribute("ACTIVIDADES")]
        ACTIVIDADES = 2
    }
}

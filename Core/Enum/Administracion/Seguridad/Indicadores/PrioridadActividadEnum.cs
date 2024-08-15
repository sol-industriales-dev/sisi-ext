using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum PrioridadActividadEnum
    {
        [DescriptionAttribute("Alta")]
        Alta = 1,
        [DescriptionAttribute("Media")]
        Media = 2,
        [DescriptionAttribute("Baja")]
        Baja = 3
    }
}

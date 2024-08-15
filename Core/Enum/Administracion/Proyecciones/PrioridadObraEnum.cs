using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Proyecciones
{
    public enum PrioridadObraEnum
    {
        [DescriptionAttribute("1")]
        ALTA = 1,
        [DescriptionAttribute("2")]
        MEDIA = 2,
        [DescriptionAttribute("3")]
        BAJA = 3,
        [DescriptionAttribute("4")]
        SIN = 4,
    }
}
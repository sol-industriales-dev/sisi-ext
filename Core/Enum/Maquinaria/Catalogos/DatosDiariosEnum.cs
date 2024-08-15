using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Catalogos
{
    public enum DatosDiariosEnum
    {
        [DescriptionAttribute("Overhaul")]
        OVERHAUL = 1,
        [DescriptionAttribute("Equipo en espera de rehabilitación ")]
        REHABILITACION = 2,
        [DescriptionAttribute("Equipo en rehabilitación en TMC")]
        REHABILITACIONTMC = 3,
        [DescriptionAttribute("Equipo disponible para obra")]
        DISPONIBLEOBRA = 4,
        [DescriptionAttribute("Equipo disponible para venta")]
        DISPONIBLEVENTA = 5,
    }
}

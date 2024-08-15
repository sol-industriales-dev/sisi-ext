using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum EstatusControlObraEnum
    {
        [DescriptionAttribute("Pesimo")]
        Pesimo = 1,
        [DescriptionAttribute("Malo")]
        Malo = 2,
        [DescriptionAttribute("Regular")]
        Regular = 3,
        [DescriptionAttribute("Aceptable")]
        Aceptable = 4,
        [DescriptionAttribute("Excediendo")]
        Excediendo = 5,
    }
}

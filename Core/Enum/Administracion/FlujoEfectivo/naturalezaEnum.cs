using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum naturalezaEnum
    {
        [DescriptionAttribute("Egreso")]
        Egreso = 1,
        [DescriptionAttribute("Ingreso")]
        Ingreso = 2,
        //[DescriptionAttribute("Debe Rojo")]
        //DebeRojo = 3,
        //[DescriptionAttribute("Haber Rojo")]
        //HaberRojo = 4,
    }
}

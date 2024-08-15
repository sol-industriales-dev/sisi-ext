using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum estatusMedidasControlEnum
    {
        [DescriptionAttribute("0%")]
        cero = 0,
        [DescriptionAttribute("25%")]
        veinticinco = 25,
        [DescriptionAttribute("50%")]
        cincuenta = 50,
        [DescriptionAttribute("75%")]
        setentaYcinco = 75,
        [DescriptionAttribute("100%")]
        cien = 100
    }
}

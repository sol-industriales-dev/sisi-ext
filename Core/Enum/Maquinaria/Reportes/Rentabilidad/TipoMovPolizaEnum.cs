using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Reportes.Rentabilidad
{
    public enum TipoMovPolizaEnum
    {
        [DescriptionAttribute("Cargo")]
        Cargo = 1,
        [DescriptionAttribute("Abono")]
        Abono = 2,
        [DescriptionAttribute("Cargo Rojo")]
        CargoRojo = 3,
        [DescriptionAttribute("Abono Rojo")]
        AbonoRojo = 4
    }
}

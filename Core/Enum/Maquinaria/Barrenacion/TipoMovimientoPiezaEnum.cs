using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Barrenacion
{
    public enum TipoMovimientoPiezaEnum
    {
        [DescriptionAttribute("Alta de pieza nueva")]
        AltaNueva = 1,
        [DescriptionAttribute("Alta de pieza reparada")]
        AltaReparada = 2,
        [DescriptionAttribute("Baja normal de pieza")]
        Baja = 3,
        [DescriptionAttribute("Baja por desecho")]
        BajaDesecho = 4,
        [DescriptionAttribute("Baja por reparación")]
        BajaReparacion = 5,
        [DescriptionAttribute("Reparación de martillo")]
        ReparacionMartillo = 6,
        [DescriptionAttribute("Alta normal de pieza")]
        Alta = 7
    }
}

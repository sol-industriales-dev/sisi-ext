using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Overhaul
{
    public enum EstadosTrackingEnum
    {
        [DescriptionAttribute("En Máquina")]
        MAQUINA = 0,
        [DescriptionAttribute("En Almacen")]
        ALMACEN = 1,
        [DescriptionAttribute("Traslado")]
        TRASLADO = 2,
        [DescriptionAttribute("Desecho")]
        DESECHO = 3,
        [DescriptionAttribute("Desarmado")]
        DESARMADO = 4,
        [DescriptionAttribute("Autorización")]
        AUTORIZACION1 = 5,
        [DescriptionAttribute("Armado")]
        RQ = 6,
        [DescriptionAttribute("Armado")]
        AUTORIZACION = 7,
        [DescriptionAttribute("Armado")]
        ARMADO = 8,
        [DescriptionAttribute("Terminado")]
        TERMINADO = 9,
        [DescriptionAttribute("Traslado Almacén")]
        TRASLADOALMACEN = 10,
        [DescriptionAttribute("Fuera de CRC")]
        FUERA = 11
    }
}


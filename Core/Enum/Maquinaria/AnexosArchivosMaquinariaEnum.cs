using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum AnexosArchivosMaquinariaEnum
    {

        [DescriptionAttribute("FACTURA")]
        FACTURA = 1,
        [DescriptionAttribute("PEDIMENTO")]
        PEDIMENTO = 2,
        [DescriptionAttribute("POILIZA DE SEGUROS")]
        POILIZA_DE_SEGUROS = 3,
        [DescriptionAttribute("TARJETA DE CIRCULACION")]
        TARJETA_DE_CIRCULACION = 4,
        [DescriptionAttribute("PERMISO ESPECIAL DE CARGA")]
        PERMISO_ESPECIAL_DE_CARGA = 5,
        [DescriptionAttribute("CERTIFICACION")]
        CERTIFICACION = 6,
        [DescriptionAttribute("CUADRO COMPARATIVO")]
        CUADRO_COMPARATIVO = 7,
        [DescriptionAttribute("CONTRATOS")]
        CONTRATOS = 8,
        [DescriptionAttribute("EVIDENCIAS")]
        EVIDENCIAS = 9,
        [DescriptionAttribute("MANTENIMIENTO")]
        MANTENIMIENTO = 10,
        [DescriptionAttribute("ANSUL")]
        ANSUL = 11
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum CtaDeudorDivEnum
    {
        /// <summary>
        /// Cuenta 1105
        /// </summary>
        [DescriptionAttribute("FONDO FIJO DE CAJA")]
        FondoFijoCaja = 1105,
        /// <summary>
        /// Cuenta 1115
        /// </summary>
        [DescriptionAttribute("INVERSIÓN")]
        Inversion = 1115,
    }
}

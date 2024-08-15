using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    /// <summary>
    /// Reservas legacy
    /// </summary>
    public enum tipoReservaEnum
    {
        [DescriptionAttribute("Pago Para Cadenas")]
        PagoEnCadenas = 1,
        [DescriptionAttribute("StandBy")]
        StandBy = 8,
        [DescriptionAttribute("Estimación")]
        Estimacion = 10,
        [DescriptionAttribute("Anticipo")]
        Anticipo = 11,
        [DescriptionAttribute("Ritchie Bros")]
        RitchieBros = 12,
        [DescriptionAttribute("Cdi Automotriz Noroeste")]
        AportacionCdiAutomotrizNoroeste = 22,
        [DescriptionAttribute("Cdi Automotriz Bajio")]
        AportacionCdiAutomotrizBajio = 23,
        [DescriptionAttribute("Cdi Alimentos")]
        AportacionCdiAlimentos = 24,
        [DescriptionAttribute("Aportacion Administración")]
        AportacionAdmin = 25,
        [DescriptionAttribute("Aguinaldos")]
        Aguinaldo = 2,
        [DescriptionAttribute("Bonos")]
        Bono = 4,
        [DescriptionAttribute("Impuestos")]
        Impuestos = 6,
        [DescriptionAttribute("De Isr")]
        DeIsr = 11,
    }
}

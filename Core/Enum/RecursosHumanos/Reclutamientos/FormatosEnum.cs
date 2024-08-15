using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Reclutamientos
{
    public enum FormatosEnum
    {
        [DescriptionAttribute("FONACOT")]
        rptFonacot = 0,
        [DescriptionAttribute("GUARDERIA")]
        rptGuarderia = 1,
        [DescriptionAttribute("LABORAL")]
        rptLaboral = 2,
        [DescriptionAttribute("LACTANCIA")]
        rptLactancia = 3,
        [DescriptionAttribute("LIBERACION")]
        rptLiberacion = 4,
        [DescriptionAttribute("PAGARE")]
        rptPagare = 5,
        [DescriptionAttribute("PRESTAMOS")]
        rptPrestamos = 6,
    }
}

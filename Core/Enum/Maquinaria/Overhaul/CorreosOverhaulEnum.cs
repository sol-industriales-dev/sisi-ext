using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Overhaul
{
    public enum CorreosOverhaulEnum
    {
        [DescriptionAttribute("control.componentes@construplan.com.mx")]
        CONTROL_COMPONENTES = 0,
        [DescriptionAttribute("jesus.garibay@construplan.com.mx")]
        ADMIN_OVERHAUL = 1,
        [DescriptionAttribute("jose.gaytan@construplan.com.mx")]
        DIRECTOR_MAQUINARIA = 2,
        [DescriptionAttribute("g.reina@construplan.com.mx")]
        DIRECTOR_SERVICIOS = 3,
        [DescriptionAttribute("jesus.garibay@construplan.com.mx")]
        GERENTE = 4,
        [DescriptionAttribute("ramon.grijalva@construplan.com.mx")]
        FACILITADOR1 = 5,
        [DescriptionAttribute("oscar.roman@construplan.com.mx")]
        GERENTE2 = 6,
    }
}

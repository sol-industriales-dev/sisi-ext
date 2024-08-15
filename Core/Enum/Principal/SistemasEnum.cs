using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum SistemasEnum : int
    {
        [DescriptionAttribute("MAQUINARIA")]
        MAQUINARIA = 1,
        [DescriptionAttribute("KUBRIX")]
        KUBRIX = 2,
        [DescriptionAttribute("CONTROL_OBRA")]
        CONTROL_OBRA = 3,
        [DescriptionAttribute("ENKONTROL")]
        ENKONTROL = 4,
        [DescriptionAttribute("SEG_ACUERDOS")]
        SEG_ACUERDOS = 6,
        [DescriptionAttribute("ENCUESTAS")]
        ENCUESTAS = 8,
        [DescriptionAttribute("PROCESOS")]
        PROCESOS = 9,
        [DescriptionAttribute("SEGURIDAD")]
        SEGURIDAD = 10,
        [DescriptionAttribute("ADMINISTRACION_FINANZAS")]
        ADMINISTRACION_FINANZAS = 11,
        [DescriptionAttribute("ALMACEN")]
        ALMACEN = 12,
        [DescriptionAttribute("LICITACIONES")]
        LICITACIONES = 13,
        [DescriptionAttribute("CONTROL_INTERNO")]
        CONTROL_INTERNO = 14,
        [DescriptionAttribute("OTROS_SERVICIOS")]
        OTROS_SERVICIOS = 15,
        [DescriptionAttribute("CH")]
        RH = 16,
        [DescriptionAttribute("GOBIERNO_CORPORATIVO")]
        GOBIERNO_CORPORATIVO = 17,
        [DescriptionAttribute("PATOOS")]
        PATOOS = 18,
        [DescriptionAttribute("BARRENACION")]
        BARRENACION = 19,
        [DescriptionAttribute("PROCESOS_INGLES")]
        PROCESOS_INGLES = 20,
        [DescriptionAttribute("PMO")]
        PMO = 21,
        [DescriptionAttribute("PRINCIPAL")]
        PRINCIPAL = 22,
        [DescriptionAttribute("TI")]
        TI = 23,
        [DescriptionAttribute("COMPRAS")]
        COMPRAS = 24
    }
}

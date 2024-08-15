using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Nomina
{
    public enum TipoRequisicionGlobalEnum
    {
        [DescriptionAttribute("COSTO SOCIAL")]
        COSTO_SOCIAL = 1,
        [DescriptionAttribute("INTERCOMPAÑÍA CPLAN-GCPLAN")]
        INTERCOMPANIA = 2,
        [DescriptionAttribute("T&L-OCSI-COSTO SOCIAL")]
        TyL_OCSI_COSTO_SOCIAL = 3,
        [DescriptionAttribute("T&L-OCSI")]
        TyL_OCSI = 4
    }
}
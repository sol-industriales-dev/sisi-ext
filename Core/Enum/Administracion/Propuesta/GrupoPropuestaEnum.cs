using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum GrupoPropuestaEnum
    {
        [DescriptionAttribute("sumaRecursos")]
        sumaRecursos = 1,
        [DescriptionAttribute("saldoFinSemana")]
        saldoFinSemana = 2,
        [DescriptionAttribute("remanente")]
        remanente = 3,
        [DescriptionAttribute("proveedor")]
        proveedor = 4,
        [DescriptionAttribute("aplicaciones")]
        aplicaciones = 5,
        [DescriptionAttribute("requerimientoObra")]
        requerimientoObra = 6,
        [DescriptionAttribute("cobroProxSemanas")]
        cobroProxSemanas = 7,
    }
}

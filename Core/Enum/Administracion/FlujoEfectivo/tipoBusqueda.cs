using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum tipoBusqueda
    {
        [DescriptionAttribute("Todos")]
        Todos = 0,
        [DescriptionAttribute("Un Concepto")]
        UnConcepto = 1,
        [DescriptionAttribute("Varios Conceptos")]
        VariosConceptos = 2,
        [DescriptionAttribute("Sin Conceptos")]
        SinConceptos = 3,
        [DescriptionAttribute("Faltante Guardar")]
        FaltanteGuardar = 4
    }
}

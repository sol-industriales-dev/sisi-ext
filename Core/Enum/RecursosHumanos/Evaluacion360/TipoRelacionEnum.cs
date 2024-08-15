using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Evaluacion360
{
    public enum TipoRelacionEnum
    {
        [DescriptionAttribute("Autoevaluación")]
        AUTOEVALUACION = 1,
        [DescriptionAttribute("Pares")]
        PARES = 2,
        [DescriptionAttribute("Clientes internos")]
        CLIENTES_INTERNOS = 3,
        [DescriptionAttribute("Colaboradores")]
        COLABORADORES = 4,
        [DescriptionAttribute("Jefe")]
        JEFE = 5
    }
}
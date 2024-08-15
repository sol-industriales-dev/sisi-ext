using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Evaluacion360
{
    public enum TipoUsuarioEnum
    {
        [DescriptionAttribute("Evaluador")]
        EVALUADOR = 1,
        [DescriptionAttribute("Evaluado")]
        EVALUADO = 2
    }
}

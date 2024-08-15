using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.SAAP
{
    public enum TipoUsuarioEnum
    {
        [DescriptionAttribute("NO ESPECIFICADO")]
        noEspecificado = 0,
        [DescriptionAttribute("CAPTURISTA")]
        capturista = 1,
        [DescriptionAttribute("EVALUADOR")]
        evaluador = 2,
    }
}

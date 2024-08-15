using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.SeguimientoCompromisos
{
    public enum TipoUsuarioSCEnum
    {
        [DescriptionAttribute("NO ESPECIFICADO")]
        noEspecificado = 0,
        [DescriptionAttribute("CAPTURISTA")]
        capturista = 1,
        [DescriptionAttribute("EVALUADOR")]
        evaluador = 2,
    }
}

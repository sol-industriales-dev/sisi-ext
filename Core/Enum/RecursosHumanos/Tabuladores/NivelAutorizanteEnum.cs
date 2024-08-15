using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Tabuladores
{
    public enum NivelAutorizanteEnum
    {
        [DescriptionAttribute("CAPITAL HUMANO")]
        CAPITAL_HUMANO = 0,
        [DescriptionAttribute("GERENTE / SUBDIRECTOR / DIRECTOR DE ÁREA")]
        GERENTE_SUBDIRECTOR_DIRECTOAREA = 1,
        [DescriptionAttribute("DIRECTOR DE LÍNEA DE NEGOCIOS")]
        DIRECTOR_LINEA_NEGOCIOS = 2,
        [DescriptionAttribute("ALTA DIRECCIÓN")]
        ALTA_DIRECCION = 3,
        [DescriptionAttribute("SOLICITANTE")]
        SOLICITANTE = 4,
        [DescriptionAttribute("RESPONSABLE CC")]
        RESPONSABLE_CC = 5
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Demandas
{
    public enum TipoArchivoDemandaEnum
    {
        [DescriptionAttribute("Finiquito negociado cerrado")]
        FINIQUITO_NEGOCIADO_CERRADO = 1,
        [DescriptionAttribute("Demanda recibida")]
        DEMANDA_RECIBIDA = 2,
        [DescriptionAttribute("Desistimiento de Demanda")]
        DESISTIMIENTO_DE_DEMANDA = 3
    }
}

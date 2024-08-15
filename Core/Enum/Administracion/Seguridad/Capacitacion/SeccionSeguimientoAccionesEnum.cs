using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum SeccionSeguimientoAccionesEnum
    {
        [DescriptionAttribute("Indicadores acciones de ciclo de trabajo")]
        INDICADORES_ACCIONES_CICLO_TRABAJO = 1,
        [DescriptionAttribute("% acciones solventadas por área")]
        PORCENTAJE_ACCIONES_SOLVENTADAS_AREA = 2,
        [DescriptionAttribute("Total de acciones solventadas por área")]
        TOTAL_ACCIONES_SOLVENTADAS_AREA = 3,
        [DescriptionAttribute("Indicadores acciones de ciclo de trabajo por área")]
        INDICADORES_ACCIONES_CICLO_TRABAJO_AREA = 4,
        [DescriptionAttribute("Histórico de acciones solventadas por área")]
        HISTORICO_ACCIONES_SOLVENTADAS_AREA = 5
    }
}

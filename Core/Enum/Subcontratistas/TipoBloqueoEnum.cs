using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Subcontratistas
{
    public enum TipoBloqueoEnum
    {
        SIN_BLOQUEO = 0,
        BLOQUEO_POR_FALTA_REGISTRO_ESPECIALIZACION = 1,
        BLOQUEO_POR_INACTIVIDAD_MENSUAL = 2,
        BLOQUEO_POR_REGISTRO_ESPECIALIZACION_VENCIDO = 3,
        BLOQUEO_POR_FALTA_CONTRATO_VIGENTE = 4
    }
}

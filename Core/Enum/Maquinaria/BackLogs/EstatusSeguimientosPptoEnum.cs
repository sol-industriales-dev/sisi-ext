using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.BackLogs
{
    public enum EstatusSeguimientosPptoEnum
    {
        [DescriptionAttribute("RECHAZADO")]
        RECHAZADO = 0,
        [DescriptionAttribute("AUTORIZADO")]
        AUTORIZADO = 1,
        [DescriptionAttribute("EN ESPERA")]
        EN_ESPERA = 2
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Reclutamientos
{
    public enum EstatusEnvioCorreoEnum
    {
        [DescriptionAttribute("NO PENDIENTE POR ENVIAR")]
        noPendientePorEnviar = 0,
        [DescriptionAttribute("ENVIADO")]
        enviado = 1,
        [DescriptionAttribute("PENDIENTE POR ENVIAR")]
        pendientePorEnviar = 2
    }
}

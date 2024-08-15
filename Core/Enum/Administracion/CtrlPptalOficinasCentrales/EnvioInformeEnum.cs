using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.CtrlPptalOficinasCentrales
{
    public enum EnvioInformeEnum
    {
        [DescriptionAttribute("ENVIO")]
        envio = 1,
        [DescriptionAttribute("NO ENVIO")]
        noEnvio = 2,
        [DescriptionAttribute("NO APLICA")]
        noAplica = 3,
    }
}

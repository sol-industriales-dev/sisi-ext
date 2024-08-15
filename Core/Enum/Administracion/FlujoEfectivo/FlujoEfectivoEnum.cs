using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum FlujoEfectivoEnum
    {
        [DescriptionAttribute("Operativo")]
        Operativo = 1,
        [DescriptionAttribute("Directo")]
        Directo = 2,
    }
}

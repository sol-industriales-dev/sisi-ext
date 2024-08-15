using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Facultamiento
{
    public enum TipoAutorizacionEnum
    {
        [DescriptionAttribute("Autoriza")]
        Autoriza = 1,
        [DescriptionAttribute("VoBo1")]
        VoBo1 = 2,
        [DescriptionAttribute("VoBo2")]
        VoBo2 = 3,
    }
}

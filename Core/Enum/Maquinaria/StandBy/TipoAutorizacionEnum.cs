using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.StandBy
{
    public enum TipoAutorizacionEnum
    {
        [DescriptionAttribute("VoBo")]
        VOBO = 1,
        [DescriptionAttribute("Autorización")]
        AUTORIZACION = 2
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.SaludOcupacional
{
    public enum AptitudEnum
    {
        [DescriptionAttribute("APTO")]
        apto = 1,
        [DescriptionAttribute("NO APTO")]
        noApto = 2,
        [DescriptionAttribute("RESTRINGIDO")]
        restringido = 3,
        [DescriptionAttribute("APTO CONDICIONADO")]
        aptoCondicionado = 4,
    }
}

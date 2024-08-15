using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum EstatusEnum : int
    {
        [DescriptionAttribute("Activo")]
        ACTIVO = 1,
        [DescriptionAttribute("Inactivo")]
        INACTIVO = 2
    }
}

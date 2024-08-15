using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum tipoOperacionEnum
    {
        [DescriptionAttribute("Autorizar")]
        Autorizar = 1,
        [DescriptionAttribute("Bloquear")]
        Bloquear = 2,
        [DescriptionAttribute("Todas")]
        Todas = 3,
    }
}

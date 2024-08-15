using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    /// <summary>
    /// Enum para identificar el tipo de firma. Ej: autorización,rechazo,etc.
    /// </summary>
    public enum TipoFirmaEnum
    {
        [DescriptionAttribute("Autorización")]
        Autorizacion = 1,
        [DescriptionAttribute("Rechazo")]
        Rechazo = 2
    }
}

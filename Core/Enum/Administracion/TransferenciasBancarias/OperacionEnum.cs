using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.TransferenciasBancarias
{
    public enum OperacionEnum
    {
        [DescriptionAttribute("NO ASIGNADO")]
        NO_ASIGNADO = 0,
        //[DescriptionAttribute("PROPIAS")]
        //propias = 1,
        [DescriptionAttribute("TERCEROS")]
        terceros = 2,
        [DescriptionAttribute("SPEI")]
        SPEI = 3,
        [DescriptionAttribute("SPID")]
        SPID = 4
    }
}

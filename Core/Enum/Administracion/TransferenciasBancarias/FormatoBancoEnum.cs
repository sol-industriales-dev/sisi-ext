using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.TransferenciasBancarias
{
    public enum FormatoBancoEnum
    {
        [DescriptionAttribute("NO ESPECIFICADO")]
        NO_ESPECIFICADO = 0,
        [DescriptionAttribute("BANORTE")]
        BANORTE = 3,
        [DescriptionAttribute("SANTANDER")]
        SANTANDER = 4
    }
}

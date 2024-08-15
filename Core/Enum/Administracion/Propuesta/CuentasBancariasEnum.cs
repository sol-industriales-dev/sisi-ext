using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum CuentasBancariasEnum
    {
        [DescriptionAttribute("BANAMEX 6694788")]
        BANAMEX = 1,
        [DescriptionAttribute("HILLO 363-6810443")]
        HILLO = 4,
        [DescriptionAttribute("BANORTE 015732324-1")]
        BANORTE = 14,
        [DescriptionAttribute("BANAMEX CTA.9091019 DLLS.")]
        BANAMEXDLL = 18,
        [DescriptionAttribute("BANAMEX 9176028 DLS")]
        BANAMEXDLL2 = 19,
        [DescriptionAttribute("BANAMEX 7009-4689544 COLORA")]
        BANAMEXCOLORADA = 103,
        [DescriptionAttribute("SCOTIABANK 11006621422")]
        SCOTIABANK = 104,
        [DescriptionAttribute("SCOTIABANK 11002006715 DLLS")]
        SCOTIABANKDLL = 105,
        [DescriptionAttribute("BANAMEX 7012-1234686")]
        BANAMEX2 = 109,
        [DescriptionAttribute("SANTANDER 6550720981-3")]
        SANTANDER = 114,
    }
}

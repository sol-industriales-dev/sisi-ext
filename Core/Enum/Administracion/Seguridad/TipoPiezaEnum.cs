using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad
{
    public enum TipoPiezaEnum
    {
        [DescriptionAttribute("EXTERIOR")]
        EXTERIOR = 1,
        [DescriptionAttribute("INTERIORES")]
        INTERIORES = 2,
        [DescriptionAttribute("MECANICO")]
        MECANICO = 3,
        [DescriptionAttribute("SEGURIDAD")]
        SEGURIDAD = 4,
        [DescriptionAttribute("DOCUMENTAL")]
        DOCUMENTAL = 5
    }
}

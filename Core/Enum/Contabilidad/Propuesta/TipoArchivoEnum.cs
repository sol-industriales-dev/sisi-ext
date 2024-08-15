using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Contabilidad.Propuesta
{
    public enum TipoArchivoEnum
    {
        [DescriptionAttribute("Xml")]
        Xml = 2,
        [DescriptionAttribute("Factura")]
        Factura = 3,
    }
}

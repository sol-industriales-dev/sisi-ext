using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Contabilidad.EstadoFinanciero
{
    public enum TipoBalanceEnum
    {
        [DescriptionAttribute("Activo")]
        ACTIVO = 1,
        [DescriptionAttribute("Pasivo")]
        PASIVO = 2
    }
}

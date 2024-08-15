using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace Core.Enum.Maquinaria
{
    public enum TipoParo2OTEnum
    {
        [DescriptionAttribute("P")]
        Preventivo = 1,
        [DescriptionAttribute("C")]
        Correctivo = 2,
        [DescriptionAttribute("PD")]
        Predictivo = 3,
    }
}

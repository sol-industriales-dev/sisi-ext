using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum TipoProrrateoReservaEnum
    {
        [DescriptionAttribute("Seleccionado")]
        Seleccionado = 1,
        [DescriptionAttribute("Automatico")]
        Automatico = 2,
    }
}

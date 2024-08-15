using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum tipoEstimacionProveedorEnum
    {
        [DescriptionAttribute("Gasto de Viaje")]
        GastoDeViaje = 1,
        [DescriptionAttribute("Comisiones")]
        Comisiones = 2,
        [DescriptionAttribute("Reembolso")]
        Reembolso = 3,
    }
}

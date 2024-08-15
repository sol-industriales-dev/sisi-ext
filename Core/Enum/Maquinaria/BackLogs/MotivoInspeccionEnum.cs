using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.BackLogs
{
    public enum MotivoInspeccionEnum
    {
        [DescriptionAttribute("Obra")]
        Obra = 0,
        [DescriptionAttribute("Venta")]
        Venta = 1
    }
}

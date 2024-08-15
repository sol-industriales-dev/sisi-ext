using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.FileManager
{
    public enum EstatusObraEnum
    {
        [DescriptionAttribute("Abierta")]
        abierta = 0,
        [DescriptionAttribute("Cerrada")]
        cerrada = 1,
        [DescriptionAttribute("Todo")]
        todo = 2
    }
}

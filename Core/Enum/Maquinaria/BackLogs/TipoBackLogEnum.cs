using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.BackLogs
{
    public enum TipoBackLogEnum
    {
        [DescriptionAttribute("Obra")]
        Obra = 1,
        [DescriptionAttribute("TMC")]
        TMC = 2
    }
}
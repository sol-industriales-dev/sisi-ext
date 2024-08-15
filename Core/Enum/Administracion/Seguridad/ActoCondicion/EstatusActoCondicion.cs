using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.ActoCondicion
{
    public enum EstatusActoCondicion
    {
        [DescriptionAttribute("Completo")]
        Completo = 1,
        [DescriptionAttribute("En proceso")]
        EnProceso = 2,
        [DescriptionAttribute("Vencido")]
        Vencido = 3
    }
}

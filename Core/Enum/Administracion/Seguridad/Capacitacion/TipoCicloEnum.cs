using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum TipoCicloEnum
    {
        [DescriptionAttribute("Periódico")]
        periodico = 1,
        [DescriptionAttribute("Liberación")]
        liberacion = 2
    }
}

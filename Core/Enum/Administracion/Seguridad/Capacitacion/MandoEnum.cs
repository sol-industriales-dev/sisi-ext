using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum MandoEnum
    {
        [DescriptionAttribute("Mando Administrativo")]
        Administrativo = 1,
        [DescriptionAttribute("Mando Medio")]
        Medio = 2,
        [DescriptionAttribute("Mando Operativo")]
        Operativo = 3
    }
}

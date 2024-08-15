using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Evaluacion
{
    public enum PeriodicidadEnum
    {
        [DescriptionAttribute("Diaria")]
        Diaria = 1,
        [DescriptionAttribute("Semanal")]
        Semanal = 2,
        [DescriptionAttribute("Quincenal")]
        Quincenal = 3,
        [DescriptionAttribute("Mensual")]
        Mensual = 4
    }
}

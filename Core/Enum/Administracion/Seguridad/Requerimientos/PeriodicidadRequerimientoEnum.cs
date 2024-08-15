using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Requerimientos
{
    public enum PeriodicidadRequerimientoEnum
    {
        [DescriptionAttribute("")]
        NoAsignado = 0,
        [DescriptionAttribute("Mensual")]
        Mensual = 1,
        [DescriptionAttribute("Trimestral")]
        Trimestral = 2,
        [DescriptionAttribute("Semestral")]
        Semestral = 3,
        [DescriptionAttribute("Anual")]
        Anual = 4
    }
}

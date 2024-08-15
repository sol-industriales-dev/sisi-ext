using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum CatAreasOperativasEnum
    {
        [DescriptionAttribute("Mantenimiento")]
        mantenimiento = 1,
        [DescriptionAttribute("Operativo")]
        operativo = 2,
        [DescriptionAttribute("Administrativo")]
        administrativo = 3
    }
}

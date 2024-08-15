using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum ClasificacionHHTEnum
    {
        [DescriptionAttribute("No Asignado")]
        NoAsignado = 0,
        [DescriptionAttribute("Mantenimiento")]
        Mantenimiento = 1,
        [DescriptionAttribute("Operativo")]
        Operativo = 2,
        [DescriptionAttribute("Administrativo")]
        Administrativo = 3,
        [DescriptionAttribute("Contratista")]
        Contratista = 4
    }
}

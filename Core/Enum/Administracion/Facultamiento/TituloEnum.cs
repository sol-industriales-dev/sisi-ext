using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Facultamiento
{
    public enum TituloEnum
    {
        [DescriptionAttribute("")]
        na = 0,
        [DescriptionAttribute("Op")]
        operador = 1,
        [DescriptionAttribute("Lic")]
        licenciado = 2,
        [DescriptionAttribute("Ing")]
        ingeniero = 3,
        [DescriptionAttribute("LAE")]
        licenciadoAdminEstratega = 4,
        [DescriptionAttribute("Doc")]
        doctor = 5,
        [DescriptionAttribute("CP")]
        contadorPublico = 6
    }
}

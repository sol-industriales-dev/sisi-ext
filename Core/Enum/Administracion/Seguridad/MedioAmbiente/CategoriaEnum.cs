using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.MedioAmbiente
{
    public enum CategoriaEnum
    {
        [DescriptionAttribute("RESIDUO")]
        residuo = 1,
        [DescriptionAttribute("CONSUMO")]
        consumo = 2
    }
}

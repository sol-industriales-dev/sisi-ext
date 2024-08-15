using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum TipoCargaGraficaEnum
    {
        [DescriptionAttribute("Año Actual")]
        Actual = 1,
        [DescriptionAttribute("Año Anterior")]
        Anterior = 2,
        [DescriptionAttribute("Año Anterior y Actual")]
        AnteriorYActual = 3
    }
}

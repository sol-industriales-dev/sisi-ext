using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.AgendarJunta
{
    public enum TipoRepeticionEnum
    {
        [DescriptionAttribute("Nunca")]
        NUNCA = 0,
        [DescriptionAttribute("Entre semana")]
        ENTRE_SEMANA = 1,
        [DescriptionAttribute("Todos los días")]
        TODOS_LOS_DIAS = 2,
        [DescriptionAttribute("Cada semana")]
        CADA_SEMANA = 3,
        [DescriptionAttribute("Cada mes")]
        CADA_MES = 4,
        [DescriptionAttribute("Cada año")]
        CADA_ANIO = 5,
        [DescriptionAttribute("Personalizado")]
        PERSONALIZADO = 6
    }
}

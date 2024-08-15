using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum ProrrateoCCEnum
    {
        [DescriptionAttribute("Stand by")]
        StandBy = 1,
        [DescriptionAttribute("Aportación Administración")]
        AportacionAdministracion = 30,
        [DescriptionAttribute("Cdi Automotriz Noroeste")]
        CdiAutomotrizNoroeste = 21,
        [DescriptionAttribute("Cdi Automotriz Bajio")]
        CdiAutomotrizBajio = 22,
        [DescriptionAttribute("Cdi Alimentos")]
        CdiAlimentos = 23,
    }
}

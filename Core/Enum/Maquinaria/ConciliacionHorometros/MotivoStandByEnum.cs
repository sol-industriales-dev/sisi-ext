using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.ConciliacionHorometros
{
    public enum MotivoStandByEnum
    {
        [DescriptionAttribute("Disponible para envió")]
        Envio = 1,
        [DescriptionAttribute("Reparación más de 3 días")]
        Reparaciion = 2,
        [DescriptionAttribute("Falta de tramo")]
        Tramo = 3,
        [DescriptionAttribute("Reparación por daño en obra anterior")]
        ObraAnterior = 4,
    }
}

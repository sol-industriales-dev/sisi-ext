using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Core.Enum.Administracion.CadenaProductiva
{
    public enum EstadoAutorizacionCadenaEnum
    {
        [DescriptionAttribute("PENDIENTE")]
        PENDIENTE = 0,
        [DescriptionAttribute("VoBo")]
        VoBo = 1,
        [DescriptionAttribute("AUTORIZADA")]
        AUTORIZADA = 2,
        [DescriptionAttribute("RECHAZADA")]
        RECHAZADA = 3,
        [DescriptionAttribute("INDEFINIDO")]
        INDEFINIDO = 4,
    }
}

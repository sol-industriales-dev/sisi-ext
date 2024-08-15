using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public enum estatusEnum : int
    {
        [DescriptionAttribute("Pendiente")]
        PENDIENTE = 1,
        [DescriptionAttribute("Autorizado")]
        AUTORIZADO = 2,
        [DescriptionAttribute("Rechazado")]
        RECHAZADO = 3
    }
}

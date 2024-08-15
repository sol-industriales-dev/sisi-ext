using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Vacaciones
{
    public enum RetardoHorarioEnum
    {
        [DescriptionAttribute("Horario de entrada")]
        HorarioEntrada = 0,
        [DescriptionAttribute("Salida a comer")]
        SalidaComer = 1,
        [DescriptionAttribute("Entrada de comer")]
        EntradaComer = 2,
        [DescriptionAttribute("Horario de salida")]
        HorarioSalida = 3,
    }
}

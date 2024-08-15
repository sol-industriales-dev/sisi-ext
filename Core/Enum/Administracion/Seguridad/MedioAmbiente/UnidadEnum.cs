using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.MedioAmbiente
{
    public enum UnidadEnum
    {
        [DescriptionAttribute("Kilogramo (kg)")]
        kilogramo = 1,
        [DescriptionAttribute("Tonelada (Ton)")]
        tonelada = 2,
        [DescriptionAttribute("Litro (Lt)")]
        litro = 3,
        [DescriptionAttribute("Metro Cúbico (M3)")]
        metroCubico = 4,
        [DescriptionAttribute("Kilowatt (kW)")]
        kilowatt = 5,
        [DescriptionAttribute("Megawatt (MW)")]
        megawatt = 6,
        [DescriptionAttribute("Pieza (Pza)")]
        pieza = 7
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.MedioAmbiente
{
    public enum FactorPeligroEnum
    {
        [DescriptionAttribute("Corrosividad (C)")]
        corrosividad = 1,
        [DescriptionAttribute("Reactividad (R)")]
        reactividad = 2,
        [DescriptionAttribute("Explosividad (E)")]
        explosividad = 3,
        [DescriptionAttribute("Toxicidad (T)")]
        toxicidad = 4,
        [DescriptionAttribute("Ambiental (Te)")]
        ambiental = 5,
        [DescriptionAttribute("Aguda (Th)")]
        aguda = 6,
        [DescriptionAttribute("Crónica (Tt)")]
        cronica = 7,
        [DescriptionAttribute("Inflamabilidad (I)")]
        inflamabilidad = 8,
        [DescriptionAttribute("Biológico-Infeccioso (B)")]
        biologicoInfeccioso = 9
    }
}

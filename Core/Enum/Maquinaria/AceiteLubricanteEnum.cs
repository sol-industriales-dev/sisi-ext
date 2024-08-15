using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum AceiteLubricanteEnum
    {
        [DescriptionAttribute("Motor")]
        MOTOR = 1,
        [DescriptionAttribute("Transmisión")]
        TRANSMISION = 2,
        [DescriptionAttribute("Hidraúlico (HCO)")]
        HIDRAULICO = 3,
        [DescriptionAttribute("Diferencial (DIF)")]
        DIFERENCIAL = 4,
        [DescriptionAttribute("Mandos Finales/Tandems (MFT)")]
        MANDOS_FINALES_TANDEMS = 5,
        [DescriptionAttribute("GRASA")]
        GRASA = 14,
        [DescriptionAttribute("Dirección (DIR)")]
        DIRECCION = 9,
        [DescriptionAttribute("Otros 1")]
        OTROS1 = 10,
        [DescriptionAttribute("Otros 2")]
        OTROS2 = 11,
        [DescriptionAttribute("Otros 3")]
        OTROS3 = 12,
        [DescriptionAttribute("Otros 4")]
        OTROS4 = 13
    }
}

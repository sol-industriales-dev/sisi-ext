using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum PeriodoFacturacionEnum
    {
        [DescriptionAttribute("Semanal")]
        Semanal = 1,
        [DescriptionAttribute("Quincenal")]
        Quincenal = 2,
        [DescriptionAttribute("Mensual")]
        Mensual = 3,      
        [DescriptionAttribute("Semestral")]
        Semestral = 4,
        [DescriptionAttribute("Anual")]
        Anual = 5
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria
{
    public enum MotivoParoOTEnum
    {
        [DescriptionAttribute("Cambio de manguera")]
        Cambio_de_manguera = 1,
        [DescriptionAttribute("Cambio de focos")]
        Cambio_de_focos = 2,
        [DescriptionAttribute("Cambio de filtros")]
        Cambio_de_filtros = 3,
        [DescriptionAttribute("Comunicación")]
        Comunicación = 4,
        [DescriptionAttribute("Elementos de ajuste")]
        Elementos_de_ajuste = 5,
        [DescriptionAttribute("Elementos de desgaste")]
        Elementos_de_desgaste = 6,
        [DescriptionAttribute("Falla eléctrica")]
        Falla_eléctrica = 7,
        [DescriptionAttribute("Diagnóstico")]
        Diagnóstico = 8,
        [DescriptionAttribute("Falla mecánica")]
        Falla_mecánica = 9,
        [DescriptionAttribute("Llantas")]
        Llantas = 10,
        [DescriptionAttribute("Fugas")]
        Fugas = 11,
        [DescriptionAttribute("Riñoneo")]
        Riñoneo = 12,
        [DescriptionAttribute("Soldadura")]
        Soldadura = 13,
        [DescriptionAttribute("A/C")]
        A_C = 14,
        [DescriptionAttribute("PM1 y Backlogs")]
        PM1_y_Backlogs = 15,
        [DescriptionAttribute("PM1 y Backlogs")]
        PM2_y_Backlogs = 16,
        [DescriptionAttribute("PM3 y Backlogs")]
        PM3_y_Backlogs = 17,
        [DescriptionAttribute("PM4 y Backlogs")]
        PM4_y_Backlogs = 18,
        [DescriptionAttribute("Lavado")]
        Lavado = 19,
        [DescriptionAttribute("Lubricación")]
        Lubricación = 20,
        [DescriptionAttribute("Implementos")]
        Implementos = 21,
        [DescriptionAttribute("Instalación de aditamentos")]
        Instalación_de_aditamentos = 22,
        [DescriptionAttribute("Remoción")]
        Remoción = 23,
        [DescriptionAttribute("Overhaul")]
        Overhaul = 24,
        [DescriptionAttribute("Mala operación")]
        Mala_operación = 25,
        [DescriptionAttribute("Otros")]
        Otros = 26
    }
}

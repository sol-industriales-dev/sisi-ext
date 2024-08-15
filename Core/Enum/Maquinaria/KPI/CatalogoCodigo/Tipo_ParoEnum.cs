using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.KPI.CatalogoCodigo
{
    public enum Tipo_ParoEnum
    {
        [DescriptionAttribute("Tiempos de Trabajo")]
        trabajo = 1,
        [DescriptionAttribute("Demoras Operativas")]
        demoras = 2,
        [DescriptionAttribute("Sin Utilizar (Parado)")]
        sin_utilizar = 3,
        [DescriptionAttribute("Mantenimiento Programado")]
        mantenimiento_programado = 4,
        [DescriptionAttribute("Mantenimiento no Programado")]
        mantenimiento_no_programado = 5

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum TipoGraficaEnum
    {
        [DescriptionAttribute("Tasa Incidencia Anual")]
        TasaAnual = 1,
        [DescriptionAttribute("Tasa Incidencia Mensual")]
        TasaMensual = 2,
        [DescriptionAttribute("Incidentes Por Mes")]
        IncidentesPorMes = 3,
        [DescriptionAttribute("TIFR")]
        TIFR = 4,
        [DescriptionAttribute("TPDFR")]
        TPDFR = 5,
    }
}

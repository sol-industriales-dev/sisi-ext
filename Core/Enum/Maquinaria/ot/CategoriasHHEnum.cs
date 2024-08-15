using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.ot
{
    public enum CategoriasHHEnum
    {
        [DescriptionAttribute("Trabajos en maquinaria (OT)")]
        TRABAJOMAQUINARIAOT = 1,
        [DescriptionAttribute("Trabajos en instalaciones")]
        TRABAJOINSTALACIONES = 2,
        [DescriptionAttribute("Limpieza")]
        LIMPIEZA = 3,
        [DescriptionAttribute("Consulta de información")]
        CONSULTAINFORMACION = 4,
        [DescriptionAttribute("Tiempo de descanso")]
        TIEMPODESCANSO = 5,
        [DescriptionAttribute("Cursos y Capacitaciones")]
        CURSOSCAPACITACIONES = 6
    }
}

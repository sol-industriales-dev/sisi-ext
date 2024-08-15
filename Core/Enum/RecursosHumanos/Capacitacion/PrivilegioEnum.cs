using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Capacitacion
{
    public enum PrivilegioEnum
    {
        [DescriptionAttribute("No Asignado")]
        NoAsignado = 0,
        /// <summary>
        /// Mirar el dashboard, crear cursos y controles de asistencias
        /// </summary>
        [DescriptionAttribute("Administrador")]
        Administrador = 1,
        /// <summary>
        /// Mirar el dashboard
        /// </summary>
        [DescriptionAttribute("Visor")]
        Visor = 2,
        /// <summary>
        /// Crear cursos y controles de asistencias
        /// </summary>
        [DescriptionAttribute("ControlDocumentos")]
        ControlDocumentos = 3,
        /// <summary>
        /// Control de asistencia
        /// </summary>
        [DescriptionAttribute("Instructor")]
        Instructor = 4,
    }
}

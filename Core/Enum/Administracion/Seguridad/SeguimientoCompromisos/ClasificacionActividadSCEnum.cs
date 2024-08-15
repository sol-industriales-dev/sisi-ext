using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.SeguimientoCompromisos
{
    public enum ClasificacionActividadSCEnum
    {
        [DescriptionAttribute("Accidentabilidad")]
        Accidentabilidad = 1,
        [DescriptionAttribute("Disciplina Operativa")]
        DisciplinaOperativa = 2,
        [DescriptionAttribute("SSMA")]
        SSMA = 3,
        [DescriptionAttribute("SSMARC")]
        SSMARC = 4,
        [DescriptionAttribute("PCRC")]
        PCRC = 5,
        [DescriptionAttribute("Seguimiento Administrativo")]
        SeguimientoAdministrativo = 6,
        [DescriptionAttribute("Otros")]
        Otros = 7
    }
}

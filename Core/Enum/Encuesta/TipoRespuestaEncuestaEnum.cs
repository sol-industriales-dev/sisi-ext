using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Encuesta
{
    public enum TipoRespuestaEncuestaEnum
    {
        [DescriptionAttribute("SISTEMA")]
        SISTEMA = 1,
        [DescriptionAttribute("TELEFONO")]
        TELEFONO = 2,
        [DescriptionAttribute("PAPEL")]
        PAPEL = 3
    }
}

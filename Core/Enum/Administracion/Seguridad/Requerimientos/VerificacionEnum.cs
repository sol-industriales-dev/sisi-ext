using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Requerimientos
{
    public enum VerificacionEnum
    {
        [DescriptionAttribute("FÍSICA")]
        FISICA = 1,
        [DescriptionAttribute("DOCUMENTAL")]
        DOCUMENTAL = 2,
        [DescriptionAttribute("REGISTRAL")]
        REGISTRAL = 3,
        [DescriptionAttribute("ENTREVISTA")]
        ENTREVISTA = 4
    }
}

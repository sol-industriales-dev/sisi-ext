using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.SaludOcupacional
{
    public enum TipoDocumentoEnum
    {
        [DescriptionAttribute("HISTORIAL CLÍNICO")]
        historialClinico = 1,
        [DescriptionAttribute("ATENCIÓN MÉDICA")]
        atencionMedica = 2
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.BackLogs
{
    public enum TipoEvidenciaEnumBL
    {
        [DescriptionAttribute("EVIDENCIA PARA LA OT")]
        evidenciaOT = 1,
        [DescriptionAttribute("EVIDENCIA DE SEGURIDAD")]
        evidenciaSeguridad = 2,
        [DescriptionAttribute("EVIDENCIA OT VACÍA")]
        evidenciaOTVacia = 3,
        [DescriptionAttribute("EVIDENCIA OT LIBERAR")]
        evidenciaOTLiberar = 4,
        [DescriptionAttribute("EVIDENCIA ORDEN BACKLOG")]
        evidenciaOrdenBL = 5
    }
}
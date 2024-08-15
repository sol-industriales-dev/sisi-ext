using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Overhaul
{
    public enum rptFallaTipoArchivoEnum
    {
        [DescriptionAttribute("SOS")]
        SOS = 1,
        [DescriptionAttribute("PSR")]
        PSR = 2,
        [DescriptionAttribute("Evidencia Fotográfica")]
        EvidenciaFotografica = 3,
        //[DescriptionAttribute("Evidencia Video")]
        //EvidenciaVideo = 4,
    }
}

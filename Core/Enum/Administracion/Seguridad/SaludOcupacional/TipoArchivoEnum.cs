using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.SaludOcupacional
{
    public enum TipoArchivoEnum
    {
        [DescriptionAttribute("IMAGEN PERSONA")]
        imagenPersona = 1,
        [DescriptionAttribute("VOBO MEDICO INTERNO")]
        voboMedicoInterno = 2,
        [DescriptionAttribute("HC FIRMADO MEDICO EXTERNO")]
        hcFirmadoMedicoExterno = 3,
        [DescriptionAttribute("ESPIROMETRIA")]
        espirometria = 4,
        [DescriptionAttribute("AUDIOMETRIA")]
        audiometria = 5,
        [DescriptionAttribute("ELECTROCARDIOGRAMA")]
        electrocardiograma = 6,
        [DescriptionAttribute("RADIOGRAFIAS")]
        radiografia = 7,
        [DescriptionAttribute("LABORATORIO")]
        laboratorio = 8,
        [DescriptionAttribute("DOCUMENTOS ADJUNTOS")]
        documentosAdjuntos = 9
    }
}

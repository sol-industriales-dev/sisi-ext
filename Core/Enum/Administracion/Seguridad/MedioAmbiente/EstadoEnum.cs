using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.MedioAmbiente
{
    public enum EstadoEnum
    {
        [DescriptionAttribute("NO ASIGNADO")]
        noAsignado = 0,
        [DescriptionAttribute("ACOPIO")]
        acopio = 1,
        [DescriptionAttribute("TRAYECTO")]
        trayecto = 2,
        [DescriptionAttribute("DESTINO FINAL")]
        destinoFinal = 3
    }
}

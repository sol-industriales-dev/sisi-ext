using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion.CincoS
{
    public enum EstatusAuditoriaDet
    {
        [DescriptionAttribute("DETECCIÓN")]
        DETECCION = 1,
        [DescriptionAttribute("MEDIDAS IMPLEMENTADAS")]
        MEDIDAS_IMPLEMENTADAS = 2,
        [DescriptionAttribute("ACCIONES REALIZADAS")]
        ACCIONES_REALIZADAS = 3
    }
}

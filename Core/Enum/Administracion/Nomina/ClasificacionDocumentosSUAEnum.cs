using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Enum.Administracion.Nomina
{
    public enum ClasificacionDocumentosSUAEnum
    {
        [DescriptionAttribute("SUA IMSS")]
        imss = 1,
        [DescriptionAttribute("SUA INFONAVIT")]
        infonavit = 2,
        [DescriptionAttribute("CÉDULA PAGO MENSUAL")]
        cedulaMensual = 3,
        [DescriptionAttribute("CÉDULA PAGO ISN")]
        cedulaIsnMensual = 4
    }
}


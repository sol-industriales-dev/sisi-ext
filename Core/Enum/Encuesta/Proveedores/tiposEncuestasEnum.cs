using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Encuesta.Proveedores
{
    public enum tiposEncuestasEnum
    {
        [DescriptionAttribute("Evaluación Continua de Proveedores")]
        EvaluaciónContinuaDeProveedores = 1,
        [DescriptionAttribute("Proveedores de Servicio")]
        ProveedoresDeServicio = 2,
        [DescriptionAttribute("Proveedores de Consignación")]
        ProveedoresDeConsignación = 3,
    }
}

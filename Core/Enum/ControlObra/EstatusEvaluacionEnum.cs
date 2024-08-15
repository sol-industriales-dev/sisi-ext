using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum EstatusEvaluacionEnum
    {
        [DescriptionAttribute("N/A")]
        NO_ASIGNADO = 0,
        [DescriptionAttribute("PENDIENTES POR ASIGNAR")]
        PENDIENTE_POR_ASIGNAR = 1,
        [DescriptionAttribute("EVALUACIÓN ASIGNADA")]
        EVALUACION_ASIGNADA = 2,
        [DescriptionAttribute("ESTATUS DE EVALUACIÓN")]
        ESTATUS_DE_EVALUACION = 3,
        [DescriptionAttribute("COMPROMISOS")]
        COMPROMISOS = 4
    }
}

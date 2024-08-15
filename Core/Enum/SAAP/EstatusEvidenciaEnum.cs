using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.SAAP
{
    public enum EstatusEvidenciaEnum
    {
        [DescriptionAttribute("NO ESPECIFICADO")]
        NoEspecificado = 0,
        [DescriptionAttribute("Sin Iniciar")]
        SinIniciar = 1,
        [DescriptionAttribute("En Progreso")]
        EnProgreso = 2,
        [DescriptionAttribute("Completo")]
        Completo = 3
    }
}

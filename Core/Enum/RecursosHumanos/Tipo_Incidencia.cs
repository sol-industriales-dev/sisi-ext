using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos
{
    public enum Tipo_Incidencia
    {
        [DescriptionAttribute("ASISTENCIA")]
        ASISTENCIA = 1,
        [DescriptionAttribute("FALTA")]
        FALTA = 2,
        [DescriptionAttribute("PERMISO_SIN_GOCE")]
        PERMISO_SIN_GOCE = 3,
        [DescriptionAttribute("PERMISO_CON_GOCE_DE_SUELDO")]
        PERMISO_CON_GOCE_DE_SUELDO = 4,
        [DescriptionAttribute("VACACIONES")]
        VACACIONES = 5,
        [DescriptionAttribute("SUSPENDIDO")]
        SUSPENDIDO = 6,
        [DescriptionAttribute("HORAS_EXTRAS")]
        HORAS_EXTRAS = 7,
        [DescriptionAttribute("PAGAR_BONO")]
        PAGAR_BONO = 8,
        [DescriptionAttribute("DIA_FESTIVO")]
        DIA_FESTIVO = 9,
        [DescriptionAttribute("INCAPACIDAD")]
        INCAPACIDAD = 10,
        [DescriptionAttribute("DESCANSO")]
        DESCANSO = 11,
        [DescriptionAttribute("COMISION_DE_TRABAJO")]
        COMISION_DE_TRABAJO = 12,
        [DescriptionAttribute("NO_APLICA")]
        NO_APLICA = 13,
        [DescriptionAttribute("NO_APLICA")]
        NO_APLICA2 = 14,
        [DescriptionAttribute("NO_APLICA")]
        NO_APLICA3 = 15,
        [DescriptionAttribute("DESCANSO_PAGADO")]
        DESCANSO_PAGADO = 16,
        [DescriptionAttribute("DESCANSO_LABORADO")]
        DESCANSO_LABORADO = 17,
        [DescriptionAttribute("BAJA")]
        BAJA = 18
        
    
    }
}

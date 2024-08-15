using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.BackLogs
{
    public enum EstatusBackLogsTMCEnum
    {
        [DescriptionAttribute("Elaboración de presupuesto (20%)")]
        ElaboracionPresupuesto = 1,
        [DescriptionAttribute("Autorizacion de presupuesto (40%)")]
        AutorizacionPresupuesto = 2,
        [DescriptionAttribute("Elaboración de OC (50%)")]
        ElaboracionOC = 3,
        [DescriptionAttribute("Suministro de Refacciones (60%)")]
        SuministroRefacciones = 4,
        [DescriptionAttribute("Rehabilitación Programada (80%)")]
        RehabilitacionProgramada = 5,
        [DescriptionAttribute("Proceso de Instalación (90%)")]
        ProcesoInstalacion = 6,
        [DescriptionAttribute("BackLogs instalado (100%)")]
        BackLogsInstalado = 7
    }
}

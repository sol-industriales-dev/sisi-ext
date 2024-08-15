using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.Reportes.Rentabilidad
{
    public enum TipoReporteEnum
    {
        [DescriptionAttribute("Costos")]
        Costos = 1,
        [DescriptionAttribute("Ingresos")]
        Ingresos = 2,
        [DescriptionAttribute("Rentabilidad")]
        Rentabilidad = 3,
        [DescriptionAttribute("Costo/Hora")]
        CostoHora = 4,
        [DescriptionAttribute("Presupuesto Maquina")]
        PresupuestoMaquina = 5,
        //[DescriptionAttribute("PresupuestoAdministrativo")]
        //PresupuestoAdministrativo = 6,
    }
}

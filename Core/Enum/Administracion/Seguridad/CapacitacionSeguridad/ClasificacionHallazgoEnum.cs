using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.CapacitacionSeguridad
{
    public enum ClasificacionHallazgoEnum
    {
        [DescriptionAttribute("Condición Insegura")]
        condicionInsegura = 1,
        [DescriptionAttribute("Acto Inseguro")]
        actoInseguro = 2,
        [DescriptionAttribute("Acción Eficiente y Segura")]
        accionEficienteSegura = 3
    }
}

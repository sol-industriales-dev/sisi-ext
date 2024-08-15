using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Evaluacion
{
    public enum CategoriaPuestoEnum
    {
        [DescriptionAttribute("Seguridad")]
        Seguridad = 1,
        [DescriptionAttribute("Contratista")]
        Contratista = 2,
        [DescriptionAttribute("Almacén")]
        Almacen = 3,
        [DescriptionAttribute("Mantenimiento")]
        Mantenimiento = 4,
        [DescriptionAttribute("Operación")]
        Operacion = 5,
        [DescriptionAttribute("Gerencia")]
        Gerencia = 6,
        [DescriptionAttribute("Patios")]
        Patios = 7,
        [DescriptionAttribute("Overhaul")]
        Overhaul = 8,
        [DescriptionAttribute("Fletes")]
        Fletes = 9,
        [DescriptionAttribute("OTR")]
        OTR = 10,
        [DescriptionAttribute("Capacitación Operativa")]
        CapacitaciónOperativa = 11
    }
}

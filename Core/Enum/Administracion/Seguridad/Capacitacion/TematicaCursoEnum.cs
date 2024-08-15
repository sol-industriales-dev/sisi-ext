using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Capacitacion
{
    public enum TematicaCursoEnum
    {
        [DescriptionAttribute("No Asignado")]
        noAsignado = 0,
        [DescriptionAttribute("Producción")]
        produccion = 1,
        [DescriptionAttribute("Mantenimiento")]
        mantenimiento = 2,
        [DescriptionAttribute("Seguridad")]
        seguridad = 3,
        [DescriptionAttribute("Almacén")]
        almacen = 4
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Facultamiento
{
    public enum EstadoPaqueteFaEnum
    {
        [DescriptionAttribute("Editando")]
        Editando = 1,
        [DescriptionAttribute("Autorización Pendiente")]
        PendienteAutorizacion = 2,
        [DescriptionAttribute("Autorizado")]
        Autorizado = 3,
        [DescriptionAttribute("Actualización Pendiente")]
        PendienteActualizar = 4
    }
}

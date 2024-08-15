using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal.Bitacoras
{
    public enum AccionEnum : int
    {
        [DescriptionAttribute("Agregar")]
        AGREGAR = 1,
        [DescriptionAttribute("Actualizar")]
        ACTUALIZAR = 2,
        [DescriptionAttribute("Eliminar")]
        ELIMINAR = 3,
        [DescriptionAttribute("Descargar")]
        DESCARGAR = 4,
        [DescriptionAttribute("Consulta")]
        CONSULTA = 5,
        [DescriptionAttribute("Reporte")]
        REPORTE = 6,
        [DescriptionAttribute("Asignar")]
        ASIGNAR = 7,
        [DescriptionAttribute("Enviar correo")]
        CORREO = 8,
        [DescriptionAttribute("Fill combo")]
        FILLCOMBO = 9,
        [DescriptionAttribute("Método general")]
        METODO_GENERAL = 10,
        [DescriptionAttribute("General excel")]
        GENERAR_EXCEL = 11
    }
}

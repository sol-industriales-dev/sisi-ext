using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Tabuladores
{
    public enum MenuTabuladoresEnum
    {
        [DescriptionAttribute("LINEA DE NEGOCIOS")]
        LINEA_DE_NEGOCIOS = 0,
        [DescriptionAttribute("CATÁLOGO DE PUESTOS")]
        CATALOGO_DE_PUESTOS = 1,
        [DescriptionAttribute("ASIGNACIÓN DE TABULADORES")]
        ASIGNACION_DE_TABULADORES = 2,
        [DescriptionAttribute("PLANTILLA DE PERSONAL")]
        PLANTILLA_DE_PERSONAL = 3,
        [DescriptionAttribute("GESTIÓN DE TABULADORES")]
        GESTION_DE_TABULADORES = 4,
        [DescriptionAttribute("GESTIÓN DE PLANTILLAS")]
        GESTION_DE_PLANTILLAS = 5,
        [DescriptionAttribute("MODIFICACIÓN TABULADORES")]
        MODIFICACION_TABULADORES = 6,
        [DescriptionAttribute("GESTIÓN DE MODIFICACION")]
        GESTION_DE_MODIFICACION = 7,
        [DescriptionAttribute("REPORTES")]
        REPORTES = 8,
        [DescriptionAttribute("GESTION REPORTES")]
        GETSION_REPORTES = 9,
    }
}

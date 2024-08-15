using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad
{
    public enum GruposSeguridadEnum
    {
        [DescriptionAttribute("CONSTRUPLAN")]
        CONSTRUPLAN = 1,
        [DescriptionAttribute("ARRENDADORA")]
        ARRENDADORA = 2,
        [DescriptionAttribute("COLOMBIA")]
        COLOMBIA = 3,
        [DescriptionAttribute("PERU")]
        PERU = 6,
        [DescriptionAttribute("GRUPO")]
        GRUPO = 0,
        [DescriptionAttribute("CONTRATISTA")]
        CONTRATISTA = 1000,
        [DescriptionAttribute("AGRUPACION CONTRATISTA")]
        agrupacionContratistas = 2000
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum AccionesEnum
    {
        
       
        [DescriptionAttribute("AGREGAR")]
        AGREGAR = 1,
        [DescriptionAttribute("EDITAR")]
        EDITAR = 2,
        [DescriptionAttribute("ELIMINAR")]
        ELIMINAR = 3,
        [DescriptionAttribute("BUSQUEDA")]
        CONSULTA = 4,
    }
}

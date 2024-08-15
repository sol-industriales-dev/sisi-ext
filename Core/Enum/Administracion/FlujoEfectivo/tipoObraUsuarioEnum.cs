using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.FlujoEfectivo
{
    public enum tipoObraUsuarioEnum
    {
        [DescriptionAttribute("CC o AC Individual")]
        Obra = 0,
        [DescriptionAttribute("Todos CC o AC")]
        Todos = 1,
        [DescriptionAttribute("Grupo CC o AC")]
        Grupo = 2,
    }
}

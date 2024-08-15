using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.CtrlPptalOficinasCentrales
{
    public enum AutorizantesEnum
    {
        // NO EXISTE PRIMER AUTORIZANTE, NOMAS NOS INDICARON A DOS.

#if DEBUG
        [DescriptionAttribute("SEGUNDO AUTORIZANTE")]
        segundoAutorizanteID = 6571, //MARTIN ZAYAS
        [DescriptionAttribute("TERCER AUTORIZANTE")]
        tercerAutorizanteID = 7939 //OMAR NUÑEZ
#else
        [DescriptionAttribute("SEGUNDO AUTORIZANTE")]
        segundoAutorizanteID = 4, //JOSE
        [DescriptionAttribute("TERCER AUTORIZANTE")]
        tercerAutorizanteID = 1164 //GERARDO REINA
#endif
    }
}

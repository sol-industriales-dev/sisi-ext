using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.Maquinaria.Mantenimiento
{
    /// <summary>
    /// 07/05/18 
    /// RAGUILAR
    /// Tipo variante de catalogos Miscelaneo vinculacion
    /// </summary>
    public enum  MisMantenimientoEnum
    {
        [DescriptionAttribute("FILTRO")]
        FILTRO = 1,
        [DescriptionAttribute("ACEITE")]
        ACEITE = 2,
        //Tipo de MArcas tblm_catfiltros
        [DescriptionAttribute("Caterpillar")]
        Caterpillar = 1,
        [DescriptionAttribute("Donaldson")]
        Donaldson = 2,
        [DescriptionAttribute("Atlas Copco")]
        AtlasCopco = 3,
    }
}

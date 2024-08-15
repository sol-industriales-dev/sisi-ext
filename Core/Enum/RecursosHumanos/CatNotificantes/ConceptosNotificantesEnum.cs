using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.CatNotificantes
{
    public enum ConceptosNotificantesEnum
    {
        [DescriptionAttribute("Taller")]
        Taller = 1,
        [DescriptionAttribute("Almacen")]
        Almacen = 2,
        [DescriptionAttribute("Contabilidad")]
        Contabilidad = 3,
        [DescriptionAttribute("Nominas")]
        Nominas = 4,
        [DescriptionAttribute("ResponsableCC")]
        ResponsableCC = 5,
        [DescriptionAttribute("Altas")]
        Altas = 6,
        [DescriptionAttribute("Bajas")]
        Bajas = 7,
        [DescriptionAttribute("CH")]
        CH = 8,
        [DescriptionAttribute("Incapacidades")]
        Incapacidades = 9,
    }
}

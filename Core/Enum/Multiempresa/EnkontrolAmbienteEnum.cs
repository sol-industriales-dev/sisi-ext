using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Multiempresa
{
    public enum EnkontrolAmbienteEnum
    {
        [DescriptionAttribute("Productivo")]
        Prod = 0,
        [DescriptionAttribute("Recurdos Humanos")]
        Rh = 1,
        [DescriptionAttribute("Pruebas")]
        Prueba = 2,
        [DescriptionAttribute("PruebaRh")]
        PruebaRh = 3,
        [DescriptionAttribute("Recurdos Humanos Arrendadora")]
        RhArre = 4,
        [DescriptionAttribute("Colombia")]
        Colombia = 5,
        [DescriptionAttribute("CP Nomina Pruebas")]
        CpRhPruebas = 6,
        [DescriptionAttribute("Recursos Humanos Construplan")]
        RhCplan = 7,
        [DescriptionAttribute("Productivo CPLAN")]
        ProdCPLAN = 8,
        [DescriptionAttribute("Productivo ARREND")]
        ProdARREND = 9,
        [DescriptionAttribute("Prueba CPLAN")]
        PruebaCPLAN = 10,
        [DescriptionAttribute("Prueba ARREND")]
        PruebaARREND = 11,
        [DescriptionAttribute("Productivo GCPLAN")]
        ProdGCPLAN = 12,
    }
}

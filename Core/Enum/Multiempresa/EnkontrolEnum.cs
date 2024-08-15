using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Multiempresa
{
    public enum EnkontrolEnum
    {
        [DescriptionAttribute("Construplan Productivo")]
        CplanProd = 0,
        [DescriptionAttribute("Construplan Recursos Humanes")]
        CplanRh = 1,
        [DescriptionAttribute("Arrendadora Productivo")]
        ArrenProd = 2,
        [DescriptionAttribute("Arrendadora Recursos Humanos")]
        ArrenRh = 3,
        [DescriptionAttribute("Prueba Cplan Productivo")]
        PruebaCplanProd = 4,
        [DescriptionAttribute("Prueba Recursos Humanos")]
        PruebaRh = 5,
        [DescriptionAttribute("Construplan EICI")]
        CplanEici = 6,
        [DescriptionAttribute("Colombia Productivo")]
        ColombiaProductivo = 7,
        [DescriptionAttribute("Prueba Construplan EICI")]
        PruebaEici = 8,
        [DescriptionAttribute("Prueba Arren ADM")]
        PruebaArrenADM = 9,
        [DescriptionAttribute("Construplan Virtual")]
        CplanVirtual = 10,
        [DescriptionAttribute("Construplan Integradora")]
        CplanIntegradora = 11,
        [DescriptionAttribute("Arrendadora Virtual")]
        ArrenVirtual = 12,
        [DescriptionAttribute("GCPLAN")]
        GCPLAN = 13
    }
}

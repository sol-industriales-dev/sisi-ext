using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    public enum TipoCCEnum
    {
        [DescriptionAttribute("Obra Cerrada Industrial")]
        ObraCerradaIndustrial = -2,
        [DescriptionAttribute("Obra Cerrada General")]
        ObraCerradaGeneral = -1,
        [DescriptionAttribute("Construcción Pesada")]
        ConstruccionPesada = 1,
        [DescriptionAttribute("Industrial")]
        Industrial = 2,
        [DescriptionAttribute("Administración")]
        Administración = 3,
        [DescriptionAttribute("Alimentos Y Bebidas")]
        AlimentosYBebidas = 4,
        [DescriptionAttribute("Automotriz")]
        Automotriz = 5,
        [DescriptionAttribute("Energía")]
        Energía = 6,
        [DescriptionAttribute("Minado Cerro Pelón y Salto")]
        CerroPelon = 7,
        [DescriptionAttribute("Inversión Edificio")]
        InversionEdificio = 8,
        [DescriptionAttribute("Minería")]
        Mineria = 9,
        [DescriptionAttribute("Minado La Colorada")]
        Colorada = 10,
        [DescriptionAttribute("Minado Noche Buena I y II")]
        NocheBuena1y2 = 11,
        [DescriptionAttribute("Minado San Agustín")]
        SanAgustin = 12,
        [DescriptionAttribute("Gastos Fininacieros Y Otros")]
        GastosFininacierosYOtros = 13,
    }
}

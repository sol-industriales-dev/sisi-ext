using System.ComponentModel;


namespace Core.Enum.Maquinaria.Barrenacion
{
    public enum TipoPiezaEnum
    {
        [DescriptionAttribute("Broca")]
        Broca = 1,
        [DescriptionAttribute("Martillo")]
        Martillo = 2,
        [DescriptionAttribute("Barra")]
        Barra = 3,
        [DescriptionAttribute("Culata")]
        Culata = 4,
        //  [DescriptionAttribute("Portabit")]
        //   Portabit = 5,
        [DescriptionAttribute("Cilindro")]
        Cilindro = 6,
        [DescriptionAttribute("BarraSegunda")]
        BarraSegunda = 7,
        [DescriptionAttribute("Zanco")]
        Zanco = 8


    }
}

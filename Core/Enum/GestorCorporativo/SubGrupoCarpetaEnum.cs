using System.ComponentModel;

namespace Core.Enum.GestorCorporativo
{
    public enum SubGrupoCarpetaEnum
    {
        [DescriptionAttribute("No aplica")]
        NA = 1,
        [DescriptionAttribute("Comercialización")]
        COM = 2,
        [DescriptionAttribute("Tecnologías de la Información")]
        TI = 3,
        [DescriptionAttribute("Recursos humanos")]
        RH = 4,
        [DescriptionAttribute("Nuevos negocios")]
        NN = 5
    }
}

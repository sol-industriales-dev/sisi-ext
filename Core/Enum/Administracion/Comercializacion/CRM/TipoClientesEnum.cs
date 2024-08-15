using System.ComponentModel;

namespace Core.Enum.Administracion.Comercializacion.CRM
{
    public enum TipoClientesEnum
    {
        [DescriptionAttribute("Sin Definir")]
        SIN_DEFINIR = 0,
        [DescriptionAttribute("(P) Principal")]
        P = 1,
        [DescriptionAttribute("(NP) Nuevo cliente principal")]
        NP = 2,
        [DescriptionAttribute("(S) Secundario")]
        S = 3,
        [DescriptionAttribute("(NS) Nuevo cliente secundario")]
        NS = 4
    }
}
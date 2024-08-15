using System.ComponentModel;


namespace Core.Enum.GestorCorporativo
{
    public enum GrupoCarpetaEnum
    {
        [DescriptionAttribute("Asamblea de accionistas")]
        Asamblea = 1,
        [DescriptionAttribute("Consejo Consultivo")]
        Consultivo = 2,
        [DescriptionAttribute("Comité de Auditoria")]
        Auditoria = 3,
        [DescriptionAttribute("Comité de Prácticas Societarias")]
        Practicas = 4,
        [DescriptionAttribute("Dirección General")]
        DireccionGeneral = 5
    }
}

using System.ComponentModel;

namespace Core.Enum.Administracion.Comercializacion.CRM
{
    public enum TipoFiltrosEnum
    {
        [DescriptionAttribute("Cliente")]
        CLIENTE = 1,
        [DescriptionAttribute("División")]
        DIVISION = 2,
        [DescriptionAttribute("Responsable")]
        RESPONSABLE = 3,
        [DescriptionAttribute("Prioridad")]
        PRIORIDAD = 4
    }
}
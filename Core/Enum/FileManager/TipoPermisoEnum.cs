using System.ComponentModel;

namespace Core.Enum.FileManager
{
    public enum TipoPermisoEnum
    {
        [DescriptionAttribute("General por división")]
        GeneralDivision = 1,
        [DescriptionAttribute("General por obra")]
        GeneralObra = 2,
        [DescriptionAttribute("Seguridad")]
        Seguridad = 3,
        [DescriptionAttribute("Administrador seguridad")]
        SeguridadAdmin = 4,
        [DescriptionAttribute("Crear subdivisión y obra")]
        CrearSubdivisionYObra = 5,
        [DescriptionAttribute("General por subdivisión")]
        GeneralSubdivision = 6,
        [DescriptionAttribute("SubContratos")]
        SubContrastos = 7,
        [DescriptionAttribute("Licitaciones")]
        Licitaciones = 8,
        [DescriptionAttribute("Admin_y_Finanzas")]
        Admin_y_Finanzas = 9,
        [DescriptionAttribute("Control_Obra")]
        Control_Obra = 10,
        [DescriptionAttribute("Produccion")]
        Produccion = 11
    }
}

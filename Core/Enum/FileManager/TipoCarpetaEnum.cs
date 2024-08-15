using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Core.Enum.FileManager
{
    public enum TipoCarpetaEnum
    {
        [DescriptionAttribute("Año")]
        Año = 1,
        [DescriptionAttribute("División")]
        Division = 2,
        [DescriptionAttribute("Subdivisión")]
        Subdivision = 3,
        [DescriptionAttribute("Obra")]
        Obra = 4,
        [DescriptionAttribute("Base Obra")]
        BaseObra = 5,
        [DescriptionAttribute("Estimaciones")]
        Estimaciones = 6,
        [DescriptionAttribute("Subcontratos")]
        Subcontratos = 7,
        [DescriptionAttribute("Normal")]
        Normal = 8,
        [DescriptionAttribute("No Aplica")]
        NA = 9,
        [DescriptionAttribute("Estimación Industrial")]
        EstimacionesIndustrial = 10,
        [DescriptionAttribute("Subcontrato Industrial")]
        SubcontratosIndustrial = 11,
        [DescriptionAttribute("Proyecto")]
        Proyecto = 12,
        [DescriptionAttribute("Contratos de renta de equipos")]
        Contrato_Renta_Equipo = 13
    }
}

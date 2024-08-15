using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Requerimientos
{
    public enum ClasificacionEnum
    {
        [DescriptionAttribute("Normativos")]
        Normativos = 1,
        [DescriptionAttribute("Normativo - Seguridad")]
        NormativoSeguridad = 2,
        [DescriptionAttribute("Normativo - Salud")]
        NormativoSalud = 3,
        [DescriptionAttribute("Normativo - Organización")]
        NormativoOrganizacion = 4,
        [DescriptionAttribute("Normativo - Específicas")]
        NormativoEspecificas = 5,
        [DescriptionAttribute("SGSST")]
        SGSST = 6,
        [DescriptionAttribute("Requerimientos del Cliente")]
        RequerimientosCliente = 7,

        [DescriptionAttribute("Plantilla Personal")]
        PlantillaPersonal = 8,
        [DescriptionAttribute("Capacitación Procesos Internos")]
        CapacitacionProcesosInternos = 9,
        [DescriptionAttribute("Proceso de Reclutamiento")]
        ProcesoReclutamiento = 10,
        [DescriptionAttribute("Seguimiento Administrativo")]
        SeguimientoAdministrativo = 11,
        [DescriptionAttribute("Campamentos")]
        Campamentos = 12,
        [DescriptionAttribute("Comedores")]
        Comedores = 13,
        [DescriptionAttribute("Identificación de Necesidades")]
        IdentificacionNecesidades = 14,
        [DescriptionAttribute("Recepción de Insumos")]
        RecepcionInsumos = 15,
        [DescriptionAttribute("Protocolo CoVid")]
        ProtocoloCovid = 16,
        [DescriptionAttribute("Instalaciones Adecuadas")]
        InstalacionesAdecuadas = 17,
        [DescriptionAttribute("Trámites Legales")]
        TramitesLegales = 18
    }
}

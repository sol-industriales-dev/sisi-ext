using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.SAAP
{
    public enum ClasificacionActividadEnum
    {
        [DescriptionAttribute("Plantilla Personal")]
        PlantillaPersonal = 1,
        [DescriptionAttribute("Capacitación Procesos Internos")]
        CapacitacionProcesosInternos = 2,
        [DescriptionAttribute("Proceso de Reclutamiento")]
        ProcesoReclutamiento = 3,
        [DescriptionAttribute("Seguimiento Administrativo")]
        SeguimientoAdministrativo = 4,
        [DescriptionAttribute("Campamentos")]
        Campamentos = 5,
        [DescriptionAttribute("Comedores")]
        Comedores = 6,
        [DescriptionAttribute("Identificación de Necesidades")]
        IdentificacionNecesidades = 7,
        [DescriptionAttribute("Recepción de Insumos")]
        RecepcionInsumos = 8,
        [DescriptionAttribute("Protocolo CoVid")]
        ProtocoloCovid = 9,
        [DescriptionAttribute("Instalaciones Adecuadas")]
        InstalacionesAdecuadas = 10,
        [DescriptionAttribute("Trámites Legales")]
        TramitesLegales = 11,
        [DescriptionAttribute("Transporte de Personal")]
        TransportePersonal = 12,
        [DescriptionAttribute("Sistemas Informáticos")]
        SistemasInformaticos = 13
    }
}

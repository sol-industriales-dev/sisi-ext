using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Subcontratistas
{
    public enum VistasEnum
    {
        [DescriptionAttribute("Administración Evaluación")]
        administracionEvaluacion = 24884,
        //[DescriptionAttribute("Gestión Firmas")]
        //gestionFirmas = 24885,
        [DescriptionAttribute("Dashboard")]
        dashboard = 24886,
        [DescriptionAttribute("Catálogos")]
        catalogos = 26900,
        //[DescriptionAttribute("Plantillas")]
        //plantillas = 25890,
        //[DescriptionAttribute("Evaluadores")]
        //evaluadores = 25891,
        //[DescriptionAttribute("Usuario Expediente")]
        //usuarioExpediente = 30011,
        //[DescriptionAttribute("Facultamiento")]
        //facultamiento = 30014,
        [DescriptionAttribute("Calendario")]
        calendario = 30048,
        //[DescriptionAttribute("Firmantes subcontratistas")]
        //firmantes = 30047
    }
}

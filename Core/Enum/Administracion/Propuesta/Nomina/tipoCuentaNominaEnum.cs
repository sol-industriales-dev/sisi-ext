using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta.Nomina
{
    public enum tipoCuentaNominaEnum
    {
            [DescriptionAttribute("FINIQUITO")]
            FINIQUITO = 2710,
        #region Construplan
            [DescriptionAttribute("CONSTRUPLAN")]
            CONSTRUPLAN = 2720,
            [DescriptionAttribute("POVISION SERVICIOS ADMINISTRATIVOS")]
            ProvisionServicioAdministrativos = 124,
            [DescriptionAttribute("SERVICIOS ADMINISTRATIVOS COMPLEMENTARIA")]
            ServiciosAdministrativosComplementaria = 42,
            [DescriptionAttribute("CONSTRUCTORA RAVELIO")]
            ConstructoraRavelio = 2762,
            [DescriptionAttribute("SONMONT")]
            SONMONT = 2821,
            [DescriptionAttribute("REGFORTE")]
            REGFORTE = 2820,
        #endregion
        #region Arrendadora
            [DescriptionAttribute("ARRENDADORA")]
            Arrendadora = 2793,
            //[DescriptionAttribute("ARRENDADORA")]
            //Arrendadora = 645,
            [DescriptionAttribute("TRONSET")]
            TRONSET = 2780,
            //[DescriptionAttribute("TRONSET")]
            //TRONSET = 693,
        #endregion
        #region EICI
        #endregion
        #region Integradora
            [DescriptionAttribute("INTEGRADORA")]
            Integradora = 2,
        #endregion
    }
}

using Core.DTO.RecursosHumanos;
using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGOPLAN.Areas.Administrativo.Models.ViewModels.RecursosHumanos
{
    public class FormatoCambiosViewModel
    {
        public List<CatFormatoCambioDTO> lstFormatosPendientes { get; set; }
        public List<CatFormatoCambioDTO> lstFormatosAutorizados { get; set; }
        public List<CatFormatoCambioDTO> lstFormatosGuardados { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class InformeDTO
    {
        public tblS_IncidentesInformePreliminar informe { get; set; }
        public List<HttpPostedFileBase> evidencias { get; set; }
    }
}

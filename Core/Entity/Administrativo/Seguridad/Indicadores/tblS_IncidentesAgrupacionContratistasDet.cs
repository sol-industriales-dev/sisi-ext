using Core.Entity.Administrativo.Contratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesAgrupacionContratistasDet
    {
        public int id { get; set; }
        public int idAgruContratista { get; set; }
        public int idContratista { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public virtual tblS_IncidentesAgrupacionContratistas agrupacionContratistas { get; set; }
        public virtual tblS_IncidentesEmpresasContratistas contratistas { get; set; }
    }
}

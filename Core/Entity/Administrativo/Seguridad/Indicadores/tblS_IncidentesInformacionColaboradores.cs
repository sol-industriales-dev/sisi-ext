using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesInformacionColaboradores
    {
        public int id { get; set; }
        public decimal horasHombre { get; set; }
        public int lostDay { get; set; }
        public string cc { get; set; }
        
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }

        [ForeignKey("idAgrupacion")]
        public virtual tblS_IncidentesAgrupacionCC agrupacion { get; set; }
    }
}

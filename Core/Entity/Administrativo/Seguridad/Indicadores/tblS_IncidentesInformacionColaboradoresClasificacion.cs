using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesInformacionColaboradoresClasificacion
    {
        public int id { get; set; }
        public string cc { get; set; }

        public DateTime fecha { get; set; }
        public decimal horasMantenimiento { get; set; }
        public decimal horasOperativo { get; set; }
        public decimal horasAdministrativo { get; set; }
        public decimal horasContratistas { get; set; }
        public int informacionColaboradoresID { get; set; }
        public virtual tblS_IncidentesInformacionColaboradores InformacionColaboradores { get; set; }
        public bool estatus { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.InsidentesContratista.AgrupacionCC
{
    public class tbls_con_incidentesAgrupacionCCDet
    {

        public int id { get; set; }
        public int idAgrupacionCC { get; set; }
        public virtual tbls_con_incidentesAgrupacionCC idAgrupacion { get; set; }
        public string cc { get; set; }
        public int idEmpresa { get; set; }
        public bool esActivo { get; set; }
        public int usuarioIDCaptura { get; set; }
        public int usuarioIDModifica { get; set; }
        public DateTime fechaCaptura { get; set; }
        public DateTime fechaModifica { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.InsidentesContratista.AgrupacionCC
{
    public class tbls_con_incidentesAgrupacionCC
    {
        public int id { get; set; }
        public string nomAgrupacion { get; set; }
        public bool esActivo { get; set; }

        public int usuarioIDCaptura { get; set; }
        public int usuarioModifica { get; set; }
        public DateTime fechaCaptura { get; set; }
        public DateTime fechaModifica { get; set; }
    }
}

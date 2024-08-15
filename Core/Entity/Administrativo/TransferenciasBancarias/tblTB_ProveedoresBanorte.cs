using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.TransferenciasBancarias
{
    public class tblTB_ProveedoresBanorte
    {
        public int id { get; set; }
        public string banorte_id { get; set; }
        public string nombre { get; set; }
        public string rfc { get; set; }
        public string contacto { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string cuenta_clabe_celular { get; set; }
        public string titular { get; set; }
        public string tipoCuenta { get; set; }
        public string banco { get; set; }
        public string moneda { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}

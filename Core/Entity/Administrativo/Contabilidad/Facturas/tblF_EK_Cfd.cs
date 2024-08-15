using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Cfd
    {
        public int id { get; set; }
        public int numcia { get; set; }
        public string pass { get; set; }
        public int cia_sucursal { get; set; }
        public int numcte { get; set; }
        public int factura { get; set; }
        public int numero_nc { get; set; }
        public DateTime fecha { get; set; }
        public int tm { get; set; }
        public int usuario { get; set; }
        public string cadena_original { get; set; }
        public string ruta_key { get; set; }
        public string ruta_certificado { get; set; }
        public string clave_privada { get; set; }
        public string sello_digital { get; set; }
        public string certificado { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

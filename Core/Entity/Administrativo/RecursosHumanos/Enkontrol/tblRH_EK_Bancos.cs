using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Bancos
    {
        public int id { get; set; }
        public int clave_banco { get; set; }
        public string desc_banco { get; set; }
        public int clave_sucursal { get; set; }
        public string nombre_sucursal { get; set; }
        public int? clave_plaza { get; set; }
        public string num_cuenta { get; set; }
        public int contrato { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}

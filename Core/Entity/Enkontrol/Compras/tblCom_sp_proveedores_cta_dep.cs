using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_sp_proveedores_cta_dep
    {
        public int id { get; set; }
        public int FK_idProv { get; set; }   
        public decimal numpro { get; set; }
        public int id_cta_dep { get; set; }
        public decimal banco { get; set; }
        public string descBanco { get; set; }
        public string cuenta { get; set; }
        public int moneda { get; set; }
        public string descMoneda { get; set; }
        public string sucursal { get; set; }
        public decimal plaza { get; set; }
        public string clabe { get; set; }
        public string tipo_cta { get; set; }
        public string descCuenta { get; set; }
        public string plastico { get; set; }
        public decimal ind_cuenta_def { get; set; }
        public decimal ind_cuenta_activa { get; set; }
        public decimal bit_valida_captura { get; set; }
        public int id_usuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int id_usuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }

    }
}

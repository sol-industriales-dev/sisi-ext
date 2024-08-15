using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_sp_proveedoresColombia
    {

        public int id { get; set; }
        public decimal numpro { get; set; }
        public string nomcorto { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public string responsable { get; set; }
        public string telefono1 { get; set; }
        public string telefono2 { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string rfc { get; set; }
        public decimal limcred { get; set; }
        public decimal tmbase { get; set; }
        public decimal condpago { get; set; }
        public decimal descuento { get; set; }
        public string moneda { get; set; }
        public string cta_bancaria { get; set; }
        public decimal tipo_prov { get; set; }
        public string cancelado { get; set; }
        public string tipo_pago { get; set; }
        public string cta_cheque { get; set; }
        public string cve_banco { get; set; }
        public string plaza_banco { get; set; }
        public string tipo_cta { get; set; }
        public string filial { get; set; }
        public DateTime? fecha_modifica_plazo_pago { get; set; }
        public decimal usuario_modifica_plazo_pago { get; set; }
        public string prov_exterior { get; set; }
        public decimal tipo_tercero { get; set; }
        public decimal tipo_operacion { get; set; }
        public string curp { get; set; }
        public string bit_factoraje { get; set; }        
        public string id_fiscal { get; set; }
        public string nacionalidad { get; set; }
        public string persona_fisica { get; set; }
        public string a_paterno { get; set; }
        public string a_materno { get; set; }
        public string a_nombre { get; set; }
        public decimal cncdirid { get; set; }
        public string pyme { get; set; }
        public string calle { get; set; }
        public string tipo_soc { get; set; }
        public string colonia { get; set; }
        public string deleg { get; set; }
        public string activo { get; set; }
        public string transfer_banco { get; set; }
        public decimal cve_medio_confir { get; set; }
        public string transfer_sant { get; set; }
        public string lada { get; set; }
        public string nombre_archivo { get; set; }
        public decimal obliga_cfd { get; set; }
        public string id_codigo_cat { get; set; }
        public decimal num_empleado { get; set; }
        public decimal num_usuario { get; set; }
        public string num_nomina { get; set; }
        public decimal cat_nomina { get; set; }
        public string beneficiario { get; set; }
        public decimal ciiu { get; set; }
        public decimal base_iva { get; set; }
        public decimal id_doc_identidad { get; set; }
        public string camara_comercio { get; set; }
        public string bit_autoretenedor { get; set; }
        public decimal id_regimen { get; set; }
        public string categoria_empleado { get; set; }
        public string sincroniza_adm { get; set; }
        public string bit_corp { get; set; }
        public string convenio_nomina { get; set; }
        public int id_usuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int id_usuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool statusAutorizacion { get; set; }
        public bool vobo { get; set; }
        public int id_usuarioVobo { get; set; }
        public DateTime? fechaVobo { get; set; }
        public bool Autorizado { get; set; }
        public int id_usuarioAutorizo { get; set; }
        public DateTime? fechaAutorizo { get; set; }
        public bool statusNotificacion { get; set; }
        public bool registroActivo { get; set; }

        [NotMapped]
        public string socios { get; set; }

    }
}

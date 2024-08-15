using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Poliza
{
    public class ClienteDTO
    {
        public int numcte { get; set; }
        public string nomcorto { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public string responsable { get; set; }
        public string telefono1 { get; set; }
        public string email { get; set; }
        public string rfc { get; set; }
        public decimal limcred { get; set; }
        public int tmbase { get; set; }
        public int condpago { get; set; }
        public string moneda { get; set; }
        public int tipo_cliente { get; set; }
        public int id_segmento { get; set; }
        public string cte_exterior { get; set; }
        public string bit_inicial { get; set; }
        public string calle { get; set; }
        public string no_exterior { get; set; }
        public string no_interior { get; set; }
        public string colonia { get; set; }
        public string tipo_credito { get; set; }
        public string referencia_fac { get; set; }
        public string tipo_impresion { get; set; }
        public string formato { get; set; }
        public string servidor_salida { get; set; }
        public string cfd_num_cta_pago { get; set; }
    }
}

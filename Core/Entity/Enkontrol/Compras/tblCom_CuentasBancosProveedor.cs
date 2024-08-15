using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_CuentasBancosProveedor
    {
        public int id { get; set; }
        public string ANEXO { get; set; }
        public string BAN_CODIGO { get; set; }
        public string CTABAN_CODIGO { get; set; }
        public string MON_CODIGO { get; set; }
        public int? TIPO_CTA { get; set; }
        public string BAN_CODIGO_INTERBAN { get; set; }
        public int? TIPO_CTA_INTERBAN { get; set; }
        public int? TIPO_CTA_CBK { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int? id_usuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }        
        public bool registroActivo { get; set; }

    }
}

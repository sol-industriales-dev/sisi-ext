using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_ReqDet_Comentarios
    {
        public int id { get; set; }    
        public string comentario { get; set; }
        public DateTime fecha { get; set; }

        public int ReqDet_id { get; set; }
        public virtual tblCom_ReqDet reqDetalle { get; set; }
    }
}

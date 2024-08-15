using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.Requisicion
{
    public class tblCom_SurtidoDet
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int partidaRequisicion { get; set; }
        public int insumo { get; set; }
        public int almacenOrigenID { get; set; }
        public int almacenDestinoID { get; set; }
        public decimal cantidad { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
        public string estadoSurtido { get; set; }
        public string tipoSurtidoDetalle { get; set; }
        public bool estatus { get; set; }

        public int surtidoID { get; set; }
        public virtual tblCom_Surtido surtido { get; set; }
    }
}

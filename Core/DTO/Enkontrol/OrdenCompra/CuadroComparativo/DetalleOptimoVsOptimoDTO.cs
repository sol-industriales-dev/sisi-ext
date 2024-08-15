using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra.CuadroComparativo
{
    public class DetalleOptimoVsOptimoDTO
    {
        public int Id { get; set; }
        public int NumeroOrdenCompra { get; set; }
        public string CC { get; set; }
        public int NumeroRequisicion { get; set; }
        public int Folio { get; set; }
        public int NumeroProveedor { get; set; }
        public tblCom_CC_Calificacion Calificacion { get; set; }

    }
}

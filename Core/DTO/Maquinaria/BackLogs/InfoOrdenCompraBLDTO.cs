using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class InfoOrdenCompraBLDTO
    {
        public tblCom_OrdenCompra orden { get; set; }
        public tblCom_OrdenCompraDet ordenDet { get; set; }
    }
}

using Core.DTO.Enkontrol.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_sp_tercero
    {
        public int id { get; set; }
        public string concepto { get; set; }
        public string descripcion { get; set; }
        public bool registroActivo { get; set; }
    }
}

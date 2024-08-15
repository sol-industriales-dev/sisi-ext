using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_Importe
    {
        public int id { get; set; }
        public bool isActivo { get; set; }
        /// <summary>
        /// ID orden de compra relacionada
        /// </summary>
        public int idOC { get; set; }
        /// <summary>
        /// Enum TipoImporte
        /// </summary>
        public int idTipo { get; set; }
        public string cc { get; set; }
        /// <summary>
        /// Número de orden de compra
        /// </summary>
        public int numero { get; set; }
        public int idMoneda { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal subTotal { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime registro { get; set; }
    }
}

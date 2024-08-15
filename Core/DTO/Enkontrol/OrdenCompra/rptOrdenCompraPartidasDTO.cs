using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class rptOrdenCompraPartidasDTO
    {
        public string partida { get; set; }
        public string insumoNumero { get; set; }
        public string insumoDescripcion { get; set; }
        public string areaCuenta { get; set; }
        public string fechaEntrega { get; set; }
        public string cantidad { get; set; }
        public string precioUnitario { get; set; }
        public string importe { get; set; }
        public int num_requisicion { get; set; }
    }
}

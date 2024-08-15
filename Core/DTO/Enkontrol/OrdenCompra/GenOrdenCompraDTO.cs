using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class GenOrdenCompraDTO
    {
        public string cc { get; set; }
        /// <summary>
        /// Número de requisición origen
        /// </summary>
        public int numero { get; set; }
        public int proveedor { get; set; }
        public List<GenPartidaOCDTO> lstPartida { get; set; }
        public bool isValida()
        {
            var isValida = true;
            if(string.IsNullOrEmpty(cc))
                isValida = false;
            if(numero <= 0)
                isValida = false;
            isValida = proveedor > 0;
            return isValida;
        }
    }
}

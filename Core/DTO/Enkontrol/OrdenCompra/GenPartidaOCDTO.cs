using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class GenPartidaOCDTO
    {
        public string cc { get; set; }
        /// <summary>
        /// Número de requisición origen
        /// </summary>
        public int numero { get; set; }
        public int partida { get; set; }
        public string partidaDesc { get; set; }
        public int moneda { get; set; }
        public decimal tc { get; set; }
        public decimal colocada { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidad_excedida_ppto { get; set; }
        public decimal cant_ordenada { get; set; }
        public decimal cant_cancelada { get; set; }
        public bool isValida()
        {
            var isValida = true;
            importe = colocada * tc * precio;
            if(string.IsNullOrEmpty(cc))
                isValida = false;
            if(numero <= 0)
                isValida = false;
            if(moneda <= 0)
                isValida = false;
            if(tc <= 0)
                isValida = false;
            if(colocada <= 0)
                isValida = false;
            if(precio <= 0)
                isValida = false;
            if(importe <= 0)
                isValida = false;
            if((cantidad + cant_ordenada + cant_cancelada) < cantidad_excedida_ppto)
                isValida = false;
            return isValida;
        }
    }
}

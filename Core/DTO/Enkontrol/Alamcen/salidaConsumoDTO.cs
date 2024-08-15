using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.DTO.Enkontrol.Alamcen
{
    public class salidaConsumoDTO
    {
        public int partidaRequisicion { get; set; }
        public int insumo { get; set; }
        public string insumoDescripcion { get; set; }
        public int almacen { get; set; }
        public string almacenDescripcion { get; set; }
        public decimal solicitado {get; set;}
        public decimal existencia { get; set; }
        public decimal solicitadoAP { get; set; }
        public decimal cantidadAP { get; set; }
        public decimal consumidoAP { get; set; }
        public decimal solicitadoAE { get; set; }
        public decimal cantidadAE { get; set; }
        public decimal consumidoAE { get; set; }
        public decimal solicitadoOC { get; set; }
        public decimal cantidadOC { get; set; }
        public decimal consumidoOC { get; set; }
        public decimal precio { get; set; }
        public string cc { get; set; }
        public int numeroReq { get; set; }
        public decimal cantidadConsumida { get; set; }
        public bool consumidoCompleto { get; set; }
    }
}

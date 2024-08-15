using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class ControlMovimientosInternosDTO
    {
        public int id { get; set; }
        public string Envio { get; set; }
        public string Destino { get; set; }
        public decimal Horometro { get; set; }
        public string Combustible { get; set; }
        public string Folio { get; set; }
        public int EconomicoID { get; set; }
        public int Estatus { get; set; }
        public string Comentario { get; set; }
        public string Bateria { get; set; }
        public string Marca2 { get; set; }
        public string Serie2 { get; set; }
        public string Registro { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int usuarioIDCaptura { get; set; }
    }
}

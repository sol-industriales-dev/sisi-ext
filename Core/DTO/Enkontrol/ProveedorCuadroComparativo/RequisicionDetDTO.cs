using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.ProveedorCuadroComparativo
{
    public class RequisicionDetDTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public int partida { get; set; }
        public int insumo { get; set; }
        public DateTime fecha_requerido { get; set; }
        public int cantidad { get; set; }
        public int cant_ordenada { get; set; }
        public int fecha_ordenada { get; set; }
        public string estatus { get; set; }
        public int cant_canelada { get; set; }
        public string referencia_1 { get; set; }
        public int cantidad_excedida_ppto { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
    }
}

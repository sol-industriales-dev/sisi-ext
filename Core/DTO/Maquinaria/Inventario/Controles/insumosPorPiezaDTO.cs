using Core.DTO.Maquinaria.Barrenacion;
using Core.Enum.Maquinaria.Barrenacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario.Controles
{
    public class insumosPorPiezaDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int insumo { get; set; }
        public decimal precioPieza { get; set; }
        public TipoPiezaEnum tipoPieza { get; set; }
    }
}

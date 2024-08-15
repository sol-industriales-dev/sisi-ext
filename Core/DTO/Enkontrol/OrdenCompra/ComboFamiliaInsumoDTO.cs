using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class ComboFamiliaInsumoDTO
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public int tipo_insumo { get; set; }
        public int grupo_insumo { get; set; }
        public string inventariado { get; set; }
    }
}

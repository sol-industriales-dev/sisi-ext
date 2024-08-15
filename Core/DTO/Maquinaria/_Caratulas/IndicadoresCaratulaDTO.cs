using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria._Caratulas
{
    public class IndicadoresCaratulaDTO
    {
        public int id { get; set; }
        public int idCC { get; set; }       
        public bool moneda { get; set; }
        public bool manoObra { get; set; }
        public decimal auxiliar { get; set; }
        public decimal indirectos { get; set; }
        public decimal manoObraporc { get; set; }
        public string descripcion { get; set; }
        public string manoobra { get; set; }
        public string Moneda { get; set; }
        public string areaCuenta { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }

    }
}

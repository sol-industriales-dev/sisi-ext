using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class ValuacionDTO
    {
        public int almacen { get; set; }
        public int insumo { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public decimal costo { get; set; }
        public decimal? cantidad { get; set; }
        public decimal? importe { get; set; }
        public int compania { get; set; }
        public string nomAlmacen { get; set; }
        public string descripcion { get; set; }
        public string descInsumo { get; set; }
        public int tipo_mov { get; set; }
        public int periodo { get; set; }
        public DateTime fecha { get; set; }
        public string cc { get; set; }

        public string minimo { get; set; }
        public string minimoDesc { get; set; }
        public decimal solicitadoPendiente { get; set; }
        public decimal cantidadTraspasar { get; set; }

        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
    }
}

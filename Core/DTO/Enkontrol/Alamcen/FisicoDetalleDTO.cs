using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.Alamcen
{
    public class FisicoDetalleDTO
    {
        public string centro_costo { get; set; }
        public int almacen { get; set; }
        public DateTime fecha { get; set; }
        public string fechaString { get; set; }
        public int insumo { get; set; }
        public string insumoDesc { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
        public int partida { get; set; }
        public string area_alm { get; set; }
        public string lado_alm { get; set; }
        public string estante_alm { get; set; }
        public string nivel_alm { get; set; }
        public int origen { get; set; }
        public int? tipo_mov { get; set; }

        public bool nuevo { get; set; }
        public bool flagInventarioCerrado { get; set; }
    }
}

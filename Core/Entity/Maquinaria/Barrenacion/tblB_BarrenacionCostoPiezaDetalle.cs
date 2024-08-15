using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_BarrenacionCostoPiezaDetalle
    {
        public int id { get; set; }
        public decimal precioUnitarioPieza  { get; set; }
        public decimal cantidadPieza { get; set; }
        public decimal? totalPieza { get; set; }
        public bool activa { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
        public int idBarrenacionCosto { get; set; }
        public int idPieza { get; set; }
        [JsonIgnore]
        public virtual tblB_CatalogoPieza pieza { get; set; }
        [JsonIgnore]
        public virtual tblB_BarrenacionCosto BarrenacionCosto { get; set; }

    }
}
